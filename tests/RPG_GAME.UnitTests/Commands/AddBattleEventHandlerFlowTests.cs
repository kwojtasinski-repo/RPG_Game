using FluentAssertions;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Battles.Actions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using RPG_GAME.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Commands
{
    public class AddBattleEventHandlerFlowTests
    {
        [Fact]
        public async Task should_create_battle_event()
        {
            var userId = Guid.NewGuid();
            var hero = await AddDefaultHero();
            var player = await AddDefaultPlayer(hero, userId);
            var enemy = await AddDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = await AddDefaultBattle(DateTime.Now, Guid.NewGuid(), map, player);
            var command = new AddBattleEvent { BattleId = battle.Id, EnemyId = enemy.Id, PlayerId = player.Id, Action = HeroAssign.Action.BASE_ATTACK };

            var battleEvent = await _handler.HandleAsync(command);

            battleEvent.Should().NotBeNull();
            battleEvent.BattleId.Should().Be(battle.Id);
            battleEvent.Action.CharacterId.Should().Be(enemy.Id);
            battleEvent.Action.Character.Should().Be(CharacterType.ENEMY.ToString());
        }

        [Fact]
        public async Task should_create_battle_event_and_create_current_battle_state()
        {
            var userId = Guid.NewGuid();
            var hero = await AddDefaultHero();
            var player = await AddDefaultPlayer(hero, userId);
            var enemy = await AddDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = await AddDefaultBattle(DateTime.Now, Guid.NewGuid(), map, player);
            var command = new AddBattleEvent { BattleId = battle.Id, EnemyId = enemy.Id, PlayerId = player.Id, Action = HeroAssign.Action.BASE_ATTACK };

            var battleEvent = await _handler.HandleAsync(command);

            battleEvent.Should().NotBeNull();
            var currentBattleState = await _currentBattleStateRepository.GetByBattleIdAsync(battle.Id);
            currentBattleState.Should().NotBeNull();
            currentBattleState.PlayerId.Should().Be(player.Id);
            currentBattleState.EnemyId.Should().Be(enemy.Id);
        }

        [Fact]
        public async Task should_create_battle_event_and_kill_enemy()
        {
            var userId = Guid.NewGuid();
            var hero = await AddDefaultHero();
            var player = await AddDefaultPlayer(hero, userId);
            var enemy = await AddDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = await AddDefaultBattle(DateTime.Now, Guid.NewGuid(), map, player);
            var command = new AddBattleEvent { BattleId = battle.Id, EnemyId = enemy.Id, PlayerId = player.Id, Action = HeroAssign.Action.BASE_ATTACK };

            var battleEvent = await _handler.HandleAsync(command);

            battleEvent.Should().NotBeNull();
            battleEvent.BattleId.Should().Be(battle.Id);
            battleEvent.Action.CharacterId.Should().Be(enemy.Id);
            battleEvent.Action.Character.Should().Be(CharacterType.ENEMY.ToString());
            battleEvent.Action.AttackInfo.Should().Be(FightAction.FIGHT_ACTION_ENEMY_IS_DEAD);
            var battleUpdated = await _battleRepository.GetAsync(battle.Id);
            battleUpdated.EnemiesKilled.Should().NotBeEmpty();
            battleUpdated.EnemiesKilled.Should().HaveCount(1);
            var currentBattleState = await _currentBattleStateRepository.GetByBattleIdAsync(battle.Id);
            currentBattleState.Should().NotBeNull();
            currentBattleState.EnemyHealth.Should().BeLessThanOrEqualTo(0);
            currentBattleState.EnemiesKilled.Should().NotBeEmpty();
            currentBattleState.EnemiesKilled.Should().HaveCount(1);
        }

        [Fact]
        public async Task should_create_battles_and_won_battle()
        {
            var userId = Guid.NewGuid();
            var hero = await AddDefaultHero();
            var player = await AddDefaultPlayer(hero, userId);
            var playerLevel = player.Level;
            var enemy = await AddDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = await AddDefaultBattle(DateTime.Now, Guid.NewGuid(), map, player);
            var command = new AddBattleEvent { BattleId = battle.Id, EnemyId = enemy.Id, PlayerId = player.Id, Action = HeroAssign.Action.BASE_ATTACK };
            var enemiesToKill = map.Enemies.Sum(e => e.Quantity);

            var battleEvent = await FightTillEnemyOrPlayerKilled(command, player.Hero.Health, map, enemiesToKill);

            battleEvent.Should().NotBeNull();
            battleEvent.Action.DamageDealt.Should().Be(0);
            var battleUpdated = await _battleRepository.GetAsync(battle.Id);
            battleUpdated.BattleInfo.Should().Be(BattleInfo.Won);
            battleUpdated.EnemiesKilled.Should().NotBeEmpty();
            battleUpdated.EnemiesKilled.Sum(e=>e.Value).Should().Be(enemiesToKill);
            var currentBattleState = await _currentBattleStateRepository.GetByBattleIdAsync(battle.Id);
            currentBattleState.Should().NotBeNull();
            currentBattleState.EnemyHealth.Should().BeLessThanOrEqualTo(0);
            currentBattleState.EnemiesKilled.Should().NotBeEmpty();
            currentBattleState.EnemiesKilled.Should().HaveCount(enemiesToKill);
            var playerUpdated = await _playerRepository.GetAsync(player.Id);
            playerUpdated.Level.Should().BeGreaterThan(playerLevel);
        }

        private async Task<BattleEventDto> FightTillEnemyOrPlayerKilled(AddBattleEvent command, int playerHealthStarted, Map map, int enemiesToKilled)
        {
            BattleEventDto battleEvent = null;
            var playerHealth = playerHealthStarted;
            var enemyHealth = 0;
            var enemiesKilled = new List<Guid>();
            var enemiesCount = enemiesToKilled;
            var enemyId = command.EnemyId;

            do
            {
                battleEvent = await _handler.HandleAsync(command);
                enemyHealth = battleEvent.Action.Health;
                playerHealth -= battleEvent.Action.DamageDealt;

                if (enemyHealth <= 0)
                {
                    enemiesKilled.Add(enemyId);
                    enemiesCount--;
                    var enemyToKilledCount = map.Enemies.SingleOrDefault(e => e.Enemy.Id == enemyId).Quantity;
                    enemyId = GetNextEnemyId(enemyToKilledCount, enemyId, enemiesKilled);
                }
            }
            while (enemiesCount > 0 && playerHealth > 0);

            Guid GetNextEnemyId(int enemyToKilledCount, Guid currentEnemy, IEnumerable<Guid> enemiesKilled)
            {
                var nextEnemy = GetNextEnemy(enemiesKilled);
                return enemyToKilledCount > enemiesKilled.Where(e => e == currentEnemy).Count()
                        ? enemyId : nextEnemy != null ? nextEnemy.Enemy.Id : currentEnemy;
            }

            Enemies GetNextEnemy(IEnumerable<Guid> enemiesKilled)
            {
                return map.Enemies.SingleOrDefault(e => !enemiesKilled.Contains(e.Enemy.Id));
            }

            return battleEvent;
        }

        private async Task<Battle> AddDefaultBattle(DateTime startDate, Guid userId, Map map, Player player)
        {
            var battle = BattleFixture.CreateBattleInProgress(startDate, userId, map, player);
            await _battleRepository.AddAsync(battle);
            return battle;
        }

        private async Task<Enemy> AddDefaultEnemy()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            await _enemyRepository.AddAsync(enemy);
            return enemy;
        }

        private async Task<Hero> AddDefaultHero()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            await _heroRepository.AddAsync(hero);
            return hero;
        }

        private async Task<Player> AddDefaultPlayer(Hero hero, Guid userId)
        {
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            await _playerRepository.AddAsync(player);
            return player;
        }

        private readonly AddBattleEventHandler _handler;
        private readonly IBattleRepository _battleRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IBattleManager _battleManager;
        private readonly IBattleEventRepository _battleEventRepository;
        private readonly IClock _clock;
        private readonly ICurrentBattleStateRepository _currentBattleStateRepository;
        private readonly IHeroRepository _heroRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IEnemyAttackManager _enemyAttackManager;
        private readonly IPlayerIncreaseStatsManager _playerIncreaseStatsManager;
        private readonly IEnemyIncreaseStatsManager _enemyIncreaseStatsManager;

        public AddBattleEventHandlerFlowTests()
        {
            _battleRepository = new BattleRepositoryStub();
            _playerRepository = new PlayerRepositoryStub();
            _clock = new ClockStub();
            _currentBattleStateRepository = new CurrentBattleStateRepositoryStub();
            _heroRepository = new HeroRepositoryStub();
            _enemyRepository = new EnemyRepositoryStub();
            _enemyAttackManager = new EnemyAttackManager();
            _playerIncreaseStatsManager = new PlayerIncreaseStatsManager(LoggerStub<PlayerIncreaseStatsManager>.Create());
            _enemyIncreaseStatsManager = new EnemyIncreaseStatsManager(LoggerStub<EnemyIncreaseStatsManager>.Create());
            _battleManager = new BattleManager(_clock, _currentBattleStateRepository, _heroRepository,
                _enemyRepository, _enemyAttackManager, _playerIncreaseStatsManager, _enemyIncreaseStatsManager);
            _battleEventRepository = new BattleEventRepositoryStub();
            _handler = new AddBattleEventHandler(_battleRepository, _playerRepository, _battleManager, _battleEventRepository);
        }
    }
}
