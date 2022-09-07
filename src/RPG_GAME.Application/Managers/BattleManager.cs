using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Battles.Actions;
using RPG_GAME.Core.Entities.Common;
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
            var enemiesToKill = battle.Map.Enemies.Sum(e => e.Quantity);
            var battleState = BattleState.Completed(battle.Id, player, _clock.CurrentDate());

            if (battle.EnemiesKilled.Sum(ek => ek.Value) != enemiesToKill)
            {
                var playerNotChanged = battle.EndBattle(_clock.CurrentDate(), BattleInfo.Lost.ToString(), battleState);
                return Task.FromResult(playerNotChanged);
            }

            var playerToUpdate = battle.EndBattle(_clock.CurrentDate(), BattleInfo.Won.ToString(), battleState);
            return Task.FromResult(playerToUpdate);
        }

        private async Task<EnemyAssign> CalculateEnemy(Battle battle, Guid enemyId, int playerLevel)
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

            var enemiesToKill = battle.Map.Enemies.Sum(e => e.Quantity);
            var currentEnemiesInBattle = battle.Map.Enemies.FirstOrDefault(e => e.Enemy.Id == enemyId);

            if (currentEnemiesInBattle is null)
            {
                throw new EnemyNotFoundException(enemyId);
            }

            var currentEnemyAssignInBattle = currentEnemiesInBattle.Enemy;
            var quantity = currentEnemiesInBattle.Quantity;

            if (battle.EnemiesKilled.Sum(e => e.Value) >= enemiesToKill)
            {
                throw new EnemiesWereKilledForBattleException(battle.Id);
            }

            var enemyKilled = battle.EnemiesKilled.Where(e => e.Key == enemyId).SingleOrDefault();

            if ((!enemyKilled.Equals(default(KeyValuePair<Guid, int>))) && enemyKilled.Value >= quantity)
            {
                throw new EnemyWasKilledException(enemyId, enemyKilled.Value);
            }

            var previousPlayerLevel = player.Level;
            var currentBattleState = await StrikeAsync(player, currentEnemyAssignInBattle, battle.Id, action);

            if (currentBattleState.EnemyHealth <= 0)
            {
                battle.AddKilledEnemy(enemyId);

                if (player.Level > previousPlayerLevel)
                {
                    _playerIncreaseStatsManager.CalculatePlayerStats(player, hero);
                    await CalculateEnemy(battle, currentEnemyAssignInBattle.Id, player.Level);
                    await _currentBattleStateRepository.UpdateAsync(currentBattleState);
                }

                UpdatePlayer(player, battle);
                
                if (battle.EnemiesKilled.Sum(ek => ek.Value) >= enemiesToKill)
                {
                    _playerIncreaseStatsManager.CalculatePlayerStats(player, hero);
                    var battleStateCompleted = BattleState.Completed(battle.Id, player, _clock.CurrentDate());
                    battle.EndBattle(_clock.CurrentDate(), BattleInfo.Won.ToString(), battleStateCompleted);
                }

                return new BattleEvent(Guid.NewGuid(), battle.Id,
                new FightAction(enemyId, CharacterType.ENEMY.ToString(),
                        currentEnemyAssignInBattle.EnemyName, 0, currentBattleState.EnemyHealth, FightAction.FIGHT_ACTION_ENEMY_IS_DEAD),
                        _clock.CurrentDate());
            }

            UpdatePlayer(player, battle);

            if (currentBattleState.PlayerCurrentHealth <= 0)
            {
                var battleStateCompleted = BattleState.Completed(battle.Id, player, _clock.CurrentDate());
                battle.EndBattle(_clock.CurrentDate(), BattleInfo.Lost.ToString(), battleStateCompleted);
            }

            return new BattleEvent(Guid.NewGuid(), battle.Id,
                new FightAction(currentEnemyAssignInBattle.Id,
                        CharacterType.ENEMY.ToString(), currentEnemyAssignInBattle.EnemyName,
                        currentBattleState.EnemyDamageDealt, currentBattleState.EnemyHealth, currentBattleState.EnemyAttackName), _clock.CurrentDate());
        }

        public async Task<CurrentBattleState> StrikeAsync(Player player, EnemyAssign enemyAssign, Guid battleId, string action)
        {
            var lastBattleState = await _currentBattleStateRepository.GetByBattleIdAsync(battleId);
            var currentBattleState = CreateCurrentBattleState(lastBattleState, battleId, player, enemyAssign);
            AttackEnemy(action, player, currentBattleState);

            if (currentBattleState.EnemyHealth <= 0)
            {
                currentBattleState.AddEnemyKilled(enemyAssign.Id);
                IncreaseExp(player, enemyAssign);
                await AddOrUpdateBattleState(currentBattleState, lastBattleState);
                return currentBattleState;
            }

            var enemyAttack = _enemyAttackManager.AttackHeroWithDamage(enemyAssign);
            var healthHero = player.Hero.Health;
            player.Hero.ChangeHealth(healthHero - enemyAttack.Damage);
            currentBattleState.MakeDamageToPlayer(enemyAttack.Damage);
            currentBattleState.EnemyDamageDealt = enemyAttack.Damage;
            currentBattleState.EnemyAttackName = enemyAttack.AttackName;
            await AddOrUpdateBattleState(currentBattleState, lastBattleState);

            return currentBattleState;
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

            if (lastBattleState.EnemyHealth <= 0)
            {
                return new CurrentBattleState(lastBattleState.Id, battleId, player.Id, player.Hero.Health, player.Level, enemyAssign.Id, enemyAssign.Health, _clock.CurrentDate(), lastBattleState.EnemiesKilled);
            }

            var currentBattleState = new CurrentBattleState(lastBattleState.Id, lastBattleState.BattleId, lastBattleState.PlayerId, lastBattleState.PlayerCurrentHealth,
               lastBattleState.PlayerLevel, lastBattleState.EnemyId, lastBattleState.EnemyHealth, _clock.CurrentDate(),
               lastBattleState.EnemiesKilled);

            return currentBattleState;
        }

        private static void AttackEnemy(string action, Player player, CurrentBattleState currentBattleState)
        {
            var damage = GetPlayerDamage(action, player);
            currentBattleState.MakeDamageToEnemy(damage);
            currentBattleState.PlayerDamageDealt = damage;
            currentBattleState.PlayerAction = action.ToLowerInvariant();
        }

        private static int GetPlayerDamage(string action, Player player)
        {
            if (action == null)
            {
                throw new InvalidAttackException(action);
            }

            if (action.ToLowerInvariant() == HeroAssign.Action.BASE_ATTACK.ToLowerInvariant())
            {
                return player.Hero.Attack;
            }

            var skills = player.Hero.Skills;

            foreach(var skill in skills)
            {
                if (skill.Name.ToLowerInvariant() == action.ToLowerInvariant())
                {
                    return skill.Attack;
                }
            }

            return 0;
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

        private void IncreaseExp(Player player, EnemyAssign enemyAssign)
        {
            var experienceToIncrease = enemyAssign.Experience;
            do
            {
                experienceToIncrease = player.IncreaseCurrentExpBy(experienceToIncrease);
            }
            while (player.CurrentExp > player.RequiredExp);
        }
    }
}
