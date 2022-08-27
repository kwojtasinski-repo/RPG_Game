using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Battles.Actions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Managers
{
    internal sealed class BattleManager : IBattleManager
    {
        private readonly IClock _clock;
        private readonly ICurrentBattleStateRepository _currentBattleStateRepository;
        private readonly IHeroRepository _heroRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IEnemyAttackManager _enemyAttackManager;
        private readonly IPlayerIncreaseStatsManager _playerIncreaseStatsManager;
        private readonly IEnemyIncreaseStatsManager _enemyIncreaseStatsManager;

        public BattleManager(IClock clock, ICurrentBattleStateRepository currentBattleStateRepository,
            IHeroRepository heroRepository, IEnemyRepository enemyRepository, IEnemyAttackManager enemyAttackManager,
            IPlayerIncreaseStatsManager playerIncreaseStatsManager, IEnemyIncreaseStatsManager enemyIncreaseStatsManager)
        {
            _clock = clock;
            _currentBattleStateRepository = currentBattleStateRepository;
            _heroRepository = heroRepository;
            _enemyRepository = enemyRepository;
            _enemyAttackManager = enemyAttackManager;
            _playerIncreaseStatsManager = playerIncreaseStatsManager;
            _enemyIncreaseStatsManager = enemyIncreaseStatsManager;
        }

        public Task<Player> CompleteBattle(Battle battle, Player player)
        {
            var enemiesToKill = battle.Map.Enemies.Count();
            
            foreach(var enemy in battle.Map.Enemies)
            {
                enemiesToKill += enemy.Quantity;
            }
            
            var battleState = BattleState.Completed(battle.Id, player, _clock.CurrentDate());

            if (battle.EnemiesKilled.Count() != enemiesToKill)
            {
                var playerNotChanged = battle.EndBattle(_clock.CurrentDate(), "Lost", battleState);
                return Task.FromResult(playerNotChanged);
            }

            var playerToUpdate = battle.EndBattle(_clock.CurrentDate(), "Won", battleState);
            return Task.FromResult(playerToUpdate);
        }

        public async Task<EnemyAssign> CalculateEnemy(Battle battle, Guid enemyId, int playerLevel)
        {

            var enemy = await _enemyRepository.GetAsync(enemyId);

            if (enemy is null)
            {
                throw new EnemyNotFoundException(enemyId);
            }

            var currentEnemyAssignInBattle = battle.Map.Enemies.Select(e => e.Enemy).FirstOrDefault(e => e.Id == enemyId);
            _enemyIncreaseStatsManager.IncreaseEnemyStats(playerLevel, currentEnemyAssignInBattle, enemy);

            return currentEnemyAssignInBattle;
        }

        public async Task<BattleEvent> CreateBattleEvent(Battle battle, Guid enemyId, Player player, string action)
        {
            if (battle.BattleInfo != BattleInfo.InProgress)
            {
                throw new CannotCreateEventForBattleWithInfoException(battle.Id, battle.BattleInfo.ToString());
            }

            var hero = await _heroRepository.GetAsync(player.Hero.Id);

            if (hero is null)
            {
                throw new HeroNotFoundException(player.Hero.Id);
            }

            var currentEnemyAssignInBattle = battle.Map.Enemies.Select(e => e.Enemy).FirstOrDefault(e => e.Id == enemyId);
            await CalculateEnemy(battle, enemyId, player.Level);

            // id enemy is needed and battles enemyKilled have to verify how many left also check current hp enemy and hero
            var lastBattleState = await _currentBattleStateRepository.GetAsync(battle.Id);
            var currentBattleState = CreateCurrentBattleState(lastBattleState, battle.Id, player, currentEnemyAssignInBattle);
            IsEnemyAlive(currentBattleState);
            DoAction(action, player, currentBattleState);
            
            if (currentBattleState.EnemyHealth <= 0)
            {
                IncreaseExpAndLevel(player, currentEnemyAssignInBattle.Experience, hero);
                UpdatePlayer(player, battle);
                await AddOrUpdateBattleState(currentBattleState, lastBattleState);

                return new BattleEvent(Guid.NewGuid(), battle.Id,
                new FightAction(enemyId, CharacterType.ENEMY.ToString(),
                        currentEnemyAssignInBattle.EnemyName, 0, currentBattleState.EnemyHealth, FightAction.FIGHT_ACTION_ENEMY_IS_DEAD),
                        _clock.CurrentDate());
            }

            var enemyAttack = _enemyAttackManager.AttackHeroWithDamage(currentEnemyAssignInBattle);
            var healthHero = player.Hero.Health;
            player.Hero.ChangeHealth(healthHero - enemyAttack.Damage);
            currentBattleState.MakeDamageToPlayer(enemyAttack.Damage);

            if (currentBattleState.PlayerCurrentHealth <= 0)
            {
                UpdatePlayer(player, battle);
                await AddOrUpdateBattleState(currentBattleState, lastBattleState);
                var battleStateCompleted = BattleState.Completed(battle.Id, player, _clock.CurrentDate());
                battle.EndBattle(_clock.CurrentDate(), BattleInfo.Lost.ToString(), battleStateCompleted);
                return new BattleEvent(Guid.NewGuid(), battle.Id,
                new FightAction(currentEnemyAssignInBattle.Id,
                        CharacterType.ENEMY.ToString(), currentEnemyAssignInBattle.EnemyName,
                        enemyAttack.Damage, currentBattleState.EnemyHealth, enemyAttack.AttackName), _clock.CurrentDate());
            }

            UpdatePlayer(player, battle);
            await AddOrUpdateBattleState(currentBattleState, lastBattleState);

            return new BattleEvent(Guid.NewGuid(), battle.Id, 
                new FightAction(currentEnemyAssignInBattle.Id, 
                        CharacterType.ENEMY.ToString(), currentEnemyAssignInBattle.EnemyName, 
                        enemyAttack.Damage, currentBattleState.EnemyHealth, enemyAttack.AttackName), _clock.CurrentDate());
        }

        private void UpdatePlayer(Player player, Battle battle)
        {
            var battleState = battle.GetBattleStateInAction();
            battleState.UpdatePlayer(player, _clock.CurrentDate());
        }

        private CurrentBattleState CreateCurrentBattleState(CurrentBattleState lastBattleState, Guid battleId, Player player, EnemyAssign enemyAssign)
        {
            if (lastBattleState is null)
            {
                return new CurrentBattleState(Guid.NewGuid(), battleId, player.Id, player.Hero.Health, player.Level, enemyAssign.Id, enemyAssign.Health, _clock.CurrentDate());
            }

            var currentBattleState = new CurrentBattleState(Guid.NewGuid(), lastBattleState.BattleId, lastBattleState.PlayerId, lastBattleState.PlayerCurrentHealth, 
                lastBattleState.PlayerCurrentHealth, lastBattleState.EnemyId, lastBattleState.EnemyHealth, _clock.CurrentDate());
            return currentBattleState;
        }

        private static void IsEnemyAlive(CurrentBattleState currentBattleState)
        {
            if (currentBattleState.EnemyHealth <= 0)
            {
                throw new InvalidOperationException("Enemy with id: '' is dead");
            }
        }

        private void DoAction(string action, Player player, CurrentBattleState currentBattleState)
        {
            if (action.ToLowerInvariant() == "heal")
            {
                HealPlayer(player, currentBattleState);
                return;
            }

            AttackEnemy(action, player, currentBattleState);
        }

        private static void HealPlayer(Player player, CurrentBattleState currentBattleState)
        {
            if (currentBattleState.PlayerCurrentHealth == player.Hero.Health)
            {
                return;
            }

            currentBattleState.HealPlayerBy(player.Hero.HealLvl);
        }

        private static void AttackEnemy(string action, Player player, CurrentBattleState currentBattleState)
        {
            var damage = GetPlayerDamage(action, player);
            currentBattleState.MakeDamageToEnemy(damage);
        }

        private static int GetPlayerDamage(string action, Player player)
        {
            if (action == null)
            {
                throw new InvalidAttackException(action);
            }

            if (action.ToLowerInvariant() == "attack")
            {
                return player.Hero.Attack;
            }

            var skills = player.Hero.Skills;

            foreach(var skill in skills)
            {
                if (skill.Name == action)
                {
                    return skill.Attack;
                }
            }

            return 0;
        }

        public EnemyAssign GetFirstEnemy(Battle battle)
        {
            return battle.Map.Enemies.Where(e => e.Enemy.Category == Core.Common.Category.Knight).Select(e => e.Enemy).FirstOrDefault();
        }

        private async Task AddOrUpdateBattleState(CurrentBattleState currentBattleState, CurrentBattleState lastBattleState)
        {
            if (lastBattleState is null)
            {
                await _currentBattleStateRepository.AddAsync(currentBattleState);
            }
            else
            {
                await _currentBattleStateRepository.UpdateAsync(currentBattleState);
            }
        }

        private void IncreaseExpAndLevel(Player player, decimal experience, Hero hero)
        {
            var experienceToIncrease = experience;
            do
            {
                var previousPlayerLevel = player.Level;
                experienceToIncrease = player.IncreaseCurrentExpBy(experienceToIncrease);

                if (player.Level > previousPlayerLevel)
                {
                    _playerIncreaseStatsManager.IncreasePlayerStats(player, hero);
                }
            }
            while (player.CurrentExp > player.RequiredExp);
        }
    }
}
