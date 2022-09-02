using FluentAssertions;
using Moq;
using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Battles;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Managers
{
    public class BattleManagerTests
    {
        [Fact]
        public async Task should_create_battle_event()
        {
            var enemy = AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battleEvent.BattleId.Should().Be(battle.Id);
            _currentBattleStateRepository.Verify(c => c.AddAsync(It.IsAny<CurrentBattleState>()), times: Times.Once);
        }

        [Fact]
        public async Task should_create_battle_event_and_return_enemy_attack()
        {
            var enemy = AddDefaultEnemy();
            enemy.ChangeHealth(new Core.Entities.Common.State<int>(5999, enemy.BaseHealth.IncreasingState));
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);
            _enemyAttackManager.Setup(e => e.AttackHeroWithDamage(It.IsAny<EnemyAssign>())).Returns(new EnemyAttackDto { AttackName = "attack", Damage = 5 });

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battleEvent.BattleId.Should().Be(battle.Id);
            battleEvent.Action.Should().NotBeNull();
            battleEvent.Action.CharacterId.Should().Be(enemy.Id);
            battleEvent.Action.AttackInfo.Should().NotBe("dead");
            _currentBattleStateRepository.Verify(c => c.AddAsync(It.IsAny<CurrentBattleState>()), times: Times.Once);
        }

        [Fact]
        public async Task should_create_battle_event_and_won_battle()
        {
            var enemy = AddDefaultEnemy();
            enemy.ChangeHealth(new Core.Entities.Common.State<int>(5, enemy.BaseHealth.IncreasingState));
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battle.BattleStates.Should().HaveCount(3);
            battle.BattleInfo.Should().Be(BattleInfo.Won);
            var battleState = battle.GetLatestBattleState();
            battleState.BattleStatus.Should().Be(BattleStatus.Completed);
            _currentBattleStateRepository.Verify(c => c.AddAsync(It.IsAny<CurrentBattleState>()), times: Times.Once);
            _currentBattleStateRepository.Verify(c => c.UpdateAsync(It.IsAny<CurrentBattleState>()), times: Times.Once);
        }

        [Fact]
        public async Task should_create_battle_event_and_lost_battle()
        {
            var enemy = AddDefaultEnemy();
            enemy.ChangeHealth(new Core.Entities.Common.State<int>(5999, enemy.BaseHealth.IncreasingState));
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = HeroAssign.Action.BASE_ATTACK;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);
            _enemyAttackManager.Setup(e => e.AttackHeroWithDamage(It.IsAny<EnemyAssign>())).Returns(new EnemyAttackDto { AttackName = "attack", Damage = 500000 });

            var battleEvent = await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            battleEvent.Should().NotBeNull();
            battle.BattleStates.Should().HaveCount(3);
            battle.BattleInfo.Should().Be(BattleInfo.Lost);
            var battleState = battle.GetLatestBattleState();
            battleState.BattleStatus.Should().Be(BattleStatus.Completed);
        }

        [Fact]
        public async Task should_level_up_when_create_battle_event()
        {
            var enemy = AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var playerLevel = player.Level;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);

            await _battleManager.CreateBattleEvent(battle, enemy.Id, player, action);

            player.Level.Should().BeGreaterThan(playerLevel);
        }

        [Fact]
        public async Task given_invalid_battle_info_when_create_battle_event_should_throw_an_exception()
        {
            var enemy = AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleAtPrepare(_clock.Object.CurrentDate(), userId, map, player);
            var expectedException = new CannotCreateEventForBattleWithInfoException(battle.Id, battle.BattleInfo.ToString());

            var exception = await Record.ExceptionAsync(() => _battleManager.CreateBattleEvent(battle, enemy.Id, player, action));

            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedException.Message);
            exception.Should().BeOfType(expectedException.GetType());
            ((CannotCreateEventForBattleWithInfoException)exception).BattleId.Should().Be(expectedException.BattleId);
            ((CannotCreateEventForBattleWithInfoException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
        }


        [Fact]
        public async Task given_not_existing_hero_when_create_battle_event_should_throw_an_exception()
        {
            var enemy = AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = HeroAssign.Action.BASE_ATTACK;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);
            var expectedException = new HeroNotFoundException(hero.Id);

            var exception = await Record.ExceptionAsync(() => _battleManager.CreateBattleEvent(battle, enemy.Id, player, action));

            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedException.Message);
            exception.Should().BeOfType(expectedException.GetType());
            ((HeroNotFoundException)exception).HeroId.Should().Be(expectedException.HeroId);
        }

        [Fact]
        public async Task given_killed_enemy_when_create_battle_event_should_throw_an_exception()
        {
            var enemy = AddDefaultEnemy();
            var enemy2 = AddDefaultEnemy();
            var quantity = 2;
            Guid userId = Guid.NewGuid();
            var map = CreateMap(new List<Enemy>() { enemy, enemy2 }, quantity);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);
            for (int i = 0; i < quantity; i++)
            {
                battle.AddKilledEnemy(enemy.Id);
            }
            var expectedException = new EnemyWasKilledException(enemy.Id, quantity);

            var exception = await Record.ExceptionAsync(() => _battleManager.CreateBattleEvent(battle, enemy.Id, player, action));

            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedException.Message);
            exception.Should().BeOfType(expectedException.GetType());
            ((EnemyWasKilledException)exception).EnemyId.Should().Be(expectedException.EnemyId);
            ((EnemyWasKilledException)exception).EnemyKilledTimes.Should().Be(expectedException.EnemyKilledTimes);
        }

        [Fact]
        public async Task given_battle_with_all_enemies_killed_when_create_event_should_throw_an_exception()
        {
            var enemy = AddDefaultEnemy();
            var enemy2 = AddDefaultEnemy();
            var quantity = 2;
            Guid userId = Guid.NewGuid();
            var map = CreateMap(new List<Enemy>() { enemy, enemy2 }, quantity);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var action = hero.Skills.First().Name;
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);
            AddAllKilledEnemiesToBattle(battle, map);
            var expectedException = new EnemiesWereKilledForBattleException(battle.Id);

            var exception = await Record.ExceptionAsync(() => _battleManager.CreateBattleEvent(battle, enemy.Id, player, action));

            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedException.Message);
            exception.Should().BeOfType(expectedException.GetType());
            ((EnemiesWereKilledForBattleException)exception).BattleId.Should().Be(expectedException.BattleId);
        }

        [Fact]
        public async Task player_should_strike_enemy_and_kill()
        {
            Guid userId = Guid.NewGuid();
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            enemy.ChangeHealth(1);
            var battleId = Guid.NewGuid();
            var action = HeroAssign.Action.BASE_ATTACK;

            var currentBattleState = await _battleManager.StrikeAsync(player, enemy, battleId, action);

            currentBattleState.Should().NotBeNull();
            currentBattleState.PlayerAction.Should().Be(action);
            currentBattleState.EnemyHealth.Should().BeLessThanOrEqualTo(0);
            currentBattleState.EnemiesKilled.Should().NotBeEmpty();
            currentBattleState.EnemiesKilled.Should().Contain(enemy.Id);
        }

        [Fact]
        public async Task player_should_strike_enemy()
        {
            Guid userId = Guid.NewGuid();
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            enemy.ChangeHealth(1000);
            var battleId = Guid.NewGuid();
            var action = HeroAssign.Action.BASE_ATTACK;
            _enemyAttackManager.Setup(e => e.AttackHeroWithDamage(It.IsAny<EnemyAssign>())).Returns(new EnemyAttackDto { AttackName = "attack", Damage = 5 });

            var currentBattleState = await _battleManager.StrikeAsync(player, enemy, battleId, action);

            currentBattleState.Should().NotBeNull();
            currentBattleState.PlayerAction.Should().Be(action);
            currentBattleState.EnemyDamageDealt.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task player_should_strike_enemy_and_be_killed()
        {
            Guid userId = Guid.NewGuid();
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            player.Hero.ChangeHealth(1);
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            enemy.ChangeHealth(1000);
            var battleId = Guid.NewGuid();
            var action = HeroAssign.Action.BASE_ATTACK;
            _enemyAttackManager.Setup(e => e.AttackHeroWithDamage(It.IsAny<EnemyAssign>())).Returns(new EnemyAttackDto { AttackName = "attack", Damage = 500000 });

            var currentBattleState = await _battleManager.StrikeAsync(player, enemy, battleId, action);

            currentBattleState.Should().NotBeNull();
            currentBattleState.PlayerAction.Should().Be(action);
            currentBattleState.PlayerCurrentHealth.Should().BeLessThanOrEqualTo(0);
            currentBattleState.EnemyDamageDealt.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task given_invalid_action_when_strike_shouldnt_do_any_dmg()
        {
            Guid userId = Guid.NewGuid();
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var battleId = Guid.NewGuid();
            var action = "test123";
            _enemyAttackManager.Setup(e => e.AttackHeroWithDamage(It.IsAny<EnemyAssign>())).Returns(new EnemyAttackDto { AttackName = "attack", Damage = 1 });

            var currentBattleState = await _battleManager.StrikeAsync(player, enemy, battleId, action);

            currentBattleState.Should().NotBeNull();
            currentBattleState.PlayerDamageDealt.Should().Be(0);
        }

        [Fact]
        public async Task given_null_action_when_strike_should_throw_an_exception()
        {
            Guid userId = Guid.NewGuid();
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            player.Hero.ChangeHealth(1000);
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var battleId = Guid.NewGuid();
            var expectedException = new InvalidAttackException(null);

            var exception = await Record.ExceptionAsync(() => _battleManager.StrikeAsync(player, enemy, battleId, null));

            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedException.Message);
            exception.Should().BeOfType(expectedException.GetType());
        }

        [Fact]
        public async Task should_complete_battle_as_won()
        {
            var enemy = AddDefaultEnemy();
            var enemy2 = AddDefaultEnemy();
            var quantity = 2;
            var map = CreateMap(new List<Enemy>() { enemy, enemy2 }, quantity);
            Guid userId = Guid.NewGuid();
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);
            AddAllKilledEnemiesToBattle(battle, map);

            await _battleManager.CompleteBattle(battle, player);

            battle.BattleInfo.Should().Be(BattleInfo.Won);
        }

        [Fact]
        public async Task should_complete_battle_as_lost()
        {
            var enemy = AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var battle = BattleFixture.CreateBattleInProgress(_clock.Object.CurrentDate(), userId, map, player);

            await _battleManager.CompleteBattle(battle, player);

            battle.BattleInfo.Should().Be(BattleInfo.Lost);
        }

        [Fact]
        public async Task given_invalid_battle_when_complete_should_throw_an_exception()
        {
            var enemy = AddDefaultEnemy();
            Guid userId = Guid.NewGuid();
            var map = CreateMap(enemy);
            var hero = AddDefaultHero();
            var player = CreatePlayer(hero, userId);
            var battle = BattleFixture.CreateBattleAtPrepare(_clock.Object.CurrentDate(), userId, map, player);
            await _battleManager.CompleteBattle(battle, player);
            var expectedException = new BattleStateAlreadyExistsException(BattleStatus.Completed);

            var exception = await Record.ExceptionAsync(() => _battleManager.CompleteBattle(battle, player));

            exception.Should().NotBeNull();
            exception.Message.Should().Be(expectedException.Message);
            exception.Should().BeOfType(expectedException.GetType());
            ((BattleStateAlreadyExistsException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
        }

        private Enemy AddDefaultEnemy()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var skillEnemy = EntitiesFixture.CreateDefaultSkillEnemy(baseAttack: 500, probability: 70);
            enemy.AddSkill(skillEnemy);
            _enemyRepository.Setup(e => e.GetAsync(enemy.Id)).ReturnsAsync(enemy);
            return enemy;
        }

        private void AddAllKilledEnemiesToBattle(Battle battle, Map map)
        {
            map.Enemies.ToList().ForEach(e =>
            {
                for (int i = 0; i < e.Quantity; i++)
                {
                    battle.AddKilledEnemy(e.Enemy.Id);
                }
            });
        }

        private Hero AddDefaultHero()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            var skillHero = EntitiesFixture.CreateDefaultSkillHero("Cyclon", 240);
            hero.AddSkill(skillHero);
            _heroRepository.Setup(h => h.GetAsync(hero.Id)).ReturnsAsync(hero);
            return hero;
        }

        private static Map CreateMap(Enemy enemy)
        {
            var enemies = EntitiesFixture.CreateEnemies(enemy.AsAssign());
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { enemies });
            return map;
        }

        private static Map CreateMap(IEnumerable<Enemy> enemies, int quantity = 1)
        {
            var listEnemies = new List<Enemies>();
            foreach (var enemy in enemies)
            {
                listEnemies.Add(EntitiesFixture.CreateEnemies(enemy.AsAssign(), quantity));
            }
            var map = EntitiesFixture.CreateDefaultMap(enemies: listEnemies);
            return map;
        }

        private static Player CreatePlayer(Hero hero, Guid userId)
        {
            return Player.Create("Player#1", hero.AsAssign(), 1000, userId);
        }

        private readonly IBattleManager _battleManager;
        private readonly Mock<IClock> _clock;
        private readonly Mock<ICurrentBattleStateRepository> _currentBattleStateRepository;
        private readonly Mock<IHeroRepository> _heroRepository;
        private readonly Mock<IEnemyRepository> _enemyRepository;
        private readonly Mock<IEnemyAttackManager> _enemyAttackManager;
        private readonly Mock<IPlayerIncreaseStatsManager> _playerIncreaseStatsManager;
        private readonly Mock<IEnemyIncreaseStatsManager> _enemyIncreaseStatsManager;

        public BattleManagerTests()
        {
            _clock = new Mock<IClock>();
            _clock.Setup(c => c.CurrentDate()).Returns(new DateTime(2022, 9, 2, 12, 30, 10));
            _currentBattleStateRepository = new Mock<ICurrentBattleStateRepository>();
            _heroRepository = new Mock<IHeroRepository>();
            _enemyRepository = new Mock<IEnemyRepository>();
            _enemyAttackManager = new Mock<IEnemyAttackManager>();
            _playerIncreaseStatsManager = new Mock<IPlayerIncreaseStatsManager>();
            _enemyIncreaseStatsManager = new Mock<IEnemyIncreaseStatsManager>();
            _battleManager = new BattleManager(_clock.Object, _currentBattleStateRepository.Object, _heroRepository.Object,
                    _enemyRepository.Object, _enemyAttackManager.Object, _playerIncreaseStatsManager.Object, _enemyIncreaseStatsManager.Object);
        }
    }
}
