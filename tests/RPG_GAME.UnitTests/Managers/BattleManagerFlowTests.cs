using FluentAssertions;
using Microsoft.Extensions.Logging;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
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

namespace RPG_GAME.UnitTests.Managers
{
    public class BattleManagerFlowTests
    {
        [Fact]
        public async Task should_create_event()
        {
            var enemy = await AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = await AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.CurrentDate(), userId, map, player);
            var dateExpected = _clock.CurrentDate();

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battleEvent.BattleId.Should().Be(battle.Id);
            var currentBattleState = await _currentBattleStateRepository.GetAsync(battle.Id);
            currentBattleState.Should().NotBeNull();
            currentBattleState.ModifiedDate.Should().Be(dateExpected);
        }

        [Fact]
        public async Task should_create_event_and_return_enemy_attack()
        {
            var enemy = await AddDefaultEnemy();
            enemy.ChangeHealth(new Core.Entities.Common.State<int>(5999, enemy.BaseHealth.IncreasingState));
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = await AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.CurrentDate(), userId, map, player);

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battleEvent.BattleId.Should().Be(battle.Id);
            battleEvent.Action.Should().NotBeNull();
            battleEvent.Action.CharacterId.Should().Be(enemy.Id);
            battleEvent.Action.AttackInfo.Should().NotBe("dead");
        }

        [Fact]
        public async Task should_create_event_and_lost_battle()
        {
            var enemy = await AddDefaultEnemy();
            enemy.ChangeHealth(new Core.Entities.Common.State<int>(5999, enemy.BaseHealth.IncreasingState));
            enemy.ChangeAttack(new Core.Entities.Common.State<int>(20000, enemy.BaseAttack.IncreasingState));
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = await AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.CurrentDate(), userId, map, player);

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battle.BattleStates.Should().HaveCount(3);
            battle.BattleInfo.Should().Be(Core.Entities.Battles.BattleInfo.Lost);
            var battleState = battle.GetLatestBattleState();
            battleState.BattleStatus.Should().Be(Core.Entities.Battles.BattleStatus.Completed);
        }

        [Fact]
        public async Task should_create_event_and_kill_enemy()
        {
            var enemy = await AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = await AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.CurrentDate(), userId, map, player);

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battleEvent.BattleId.Should().Be(battle.Id);
            battleEvent.Action.Should().NotBeNull();
            battleEvent.Action.CharacterId.Should().Be(enemy.Id);
            battleEvent.Action.AttackInfo.Should().Be("dead");
            var currentBattleState = await _currentBattleStateRepository.GetAsync(battle.Id);
            currentBattleState.Should().NotBeNull();
            currentBattleState.EnemyHealth.Should().BeLessThanOrEqualTo(0);
        }

        [Fact]
        public async Task should_level_up()
        {
            var enemy = await AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = await AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var playerLevel = player.Level;
            var battle = BattleFixture.CreateBattleInProgress(_clock.CurrentDate(), userId, map, player);

            await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            player.Level.Should().BeGreaterThan(playerLevel);
        }

        [Fact]
        public async Task should_create_battle_events()
        {
            var enemy = await AddDefaultEnemy();
            enemy.ChangeHealth(new Core.Entities.Common.State<int>(5999, enemy.BaseHealth.IncreasingState));
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = await AddDefaultHero();
            hero.Health.ChangeValue(2000);
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.CurrentDate(), userId, map, player);
            var battleEvents = new List<BattleEvent>();
            var battleEventFirst = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);
            var battleEventSecond = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvents.AddRange(new List<BattleEvent> { battleEventFirst, battleEventSecond });

            battleEvents.Should().NotBeNull();
            battleEvents.Should().NotBeEmpty();
            battleEvents.Should().HaveCount(2);
        }

        private async Task<Enemy> AddDefaultEnemy()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var skillEnemy = EntitiesFixture.CreateDefaultSkillEnemy(baseAttack: 500, probability: 70);
            enemy.AddSkill(skillEnemy);
            await _enemyRepository.AddAsync(enemy);
            return enemy;
        }

        private async Task<Hero> AddDefaultHero()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            var skillHero = EntitiesFixture.CreateDefaultSkillHero("Cyclon", 240);
            hero.AddSkill(skillHero);
            await _heroRepository.AddAsync(hero);
            return hero;
        }

        private static Map CreateMap(Enemy enemy)
        {
            var enemies = EntitiesFixture.CreateEnemies(enemy.AsAssign());
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { enemies });
            return map;
        }

        private static Player CreatePlayer(Hero hero, Guid userId)
        {
            return Player.Create("Player#1", hero.AsAssign(), 1000, userId);
        }

        private readonly IBattleManager _battleManager;
        private readonly IClock _clock;
        private readonly ICurrentBattleStateRepository _currentBattleStateRepository;
        private readonly IHeroRepository _heroRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IEnemyAttackManager _enemyAttackManager;
        private readonly IPlayerIncreaseStatsManager _playerIncreaseStatsManager;
        private readonly IEnemyIncreaseStatsManager _enemyIncreaseStatsManager;

        public BattleManagerFlowTests()
        {
            _clock = new ClockStub();
            _currentBattleStateRepository = new CurrentBattleStateRepositoryStub();
            _heroRepository = new HeroRepositoryStub();
            _enemyRepository = new EnemyRepositoryStub();
            _enemyAttackManager = new EnemyAttackManager();
            _playerIncreaseStatsManager = new PlayerIncreaseStatsManager(LoggerStub<PlayerIncreaseStatsManager>.Create());
            _enemyIncreaseStatsManager = new EnemyIncreaseStatsManager(LoggerStub<EnemyIncreaseStatsManager>.Create());
            _battleManager = new BattleManager(_clock, _currentBattleStateRepository, _heroRepository,
                    _enemyRepository, _enemyAttackManager, _playerIncreaseStatsManager, _enemyIncreaseStatsManager);
        }
    }
}
