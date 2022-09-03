using FluentAssertions;
using Moq;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Exceptions.Maps;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
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
    public class PrepareBattleHandlerTests
    {
        [Fact]
        public async Task should_create_prepare_battle()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            _enemyRepository.Setup(e => e.GetAsync(enemy.Id)).ReturnsAsync(enemy);
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            _mapRepository.Setup(p => p.GetAsync(map.Id)).ReturnsAsync(map);
            var hero = EntitiesFixture.CreateDefaultHero();
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            _playerRepository.Setup(p => p.GetByUserId(userId)).ReturnsAsync(player);
            var command = new PrepareBattle { MapId = map.Id, UserId = userId };

            var battleDetails = await _commandHandler.HandleAsync(command);

            _battleRepository.Verify(b => b.AddAsync(It.IsAny<Battle>()), times: Times.Once);
            battleDetails.Should().NotBeNull();
            battleDetails.UserId.Should().Be(userId);
            battleDetails.Map.Id.Should().Be(map.Id);
            battleDetails.BattleStates.Should().HaveCount(1);
            battleDetails.BattleStates.Should().Contain(bs => bs.BattleStatus == BattleStatus.Prepare.ToString());
        }

        [Fact]
        public async Task should_create_prepare_battle_with_calculated_enemies()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            _enemyRepository.Setup(e => e.GetAsync(enemy.Id)).ReturnsAsync(enemy);
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            _mapRepository.Setup(p => p.GetAsync(map.Id)).ReturnsAsync(map);
            var hero = EntitiesFixture.CreateDefaultHero();
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var player = new Player(Guid.NewGuid(), "player", hero.AsAssign(), 5, decimal.Zero, 10000M, userId);
            _playerRepository.Setup(p => p.GetByUserId(userId)).ReturnsAsync(player);
            var command = new PrepareBattle { MapId = map.Id, UserId = userId };
            var calculatedAttack = 104;
            var calculatedHealth = 104;
            var calculatedExperience = 1004;

            var battleDetails = await _commandHandler.HandleAsync(command);

            battleDetails.Should().NotBeNull();
            var enemyCalculated = battleDetails.Map.Enemies.Single(e => e.Enemy.Id == enemy.Id);
            enemyCalculated.Enemy.BaseAttack.Should().Be(calculatedAttack);
            enemyCalculated.Enemy.BaseHealth.Should().Be(calculatedHealth);
            enemyCalculated.Enemy.Experience.Should().Be(calculatedExperience);
        }

        [Fact]
        public async Task given_not_existing_user_when_prepare_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            var mapId = Guid.NewGuid();
            var command = new PrepareBattle { MapId = mapId, UserId = userId };
            var expectedException = new UserNotFoundException(userId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((UserNotFoundException)exception).UserId.Should().Be(expectedException.UserId);
        }

        [Fact]
        public async Task given_not_existing_player_for_user_id_when_prepare_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var mapId = Guid.NewGuid();
            var command = new PrepareBattle { MapId = mapId, UserId = userId };
            var expectedException = new PlayerForUserNotFoundException(userId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((PlayerForUserNotFoundException)exception).UserId.Should().Be(expectedException.UserId);
        }

        [Fact]
        public async Task given_not_existing_map_when_prepare_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var mapId = Guid.NewGuid();
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            _playerRepository.Setup(p => p.GetByUserId(userId)).ReturnsAsync(player);
            var command = new PrepareBattle { MapId = mapId, UserId = userId };
            var expectedException = new MapNotFoundException(mapId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((MapNotFoundException)exception).MapId.Should().Be(expectedException.MapId);
        }

        [Fact]
        public async Task given_map_with_no_enemies_when_prepare_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var map = EntitiesFixture.CreateDefaultMap();
            _mapRepository.Setup(p => p.GetAsync(map.Id)).ReturnsAsync(map);
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            _playerRepository.Setup(p => p.GetByUserId(userId)).ReturnsAsync(player);
            var command = new PrepareBattle { MapId = map.Id, UserId = userId };
            var expectedException = new CannotPrepareBattleForMapWithEmptyEnemiesException(map.Id);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotPrepareBattleForMapWithEmptyEnemiesException)exception).MapId.Should().Be(expectedException.MapId);
        }

        [Fact]
        public async Task given_map_with_not_existing_enemy_when_prepare_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            _mapRepository.Setup(p => p.GetAsync(map.Id)).ReturnsAsync(map);
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            _playerRepository.Setup(p => p.GetByUserId(userId)).ReturnsAsync(player);
            var command = new PrepareBattle { MapId = map.Id, UserId = userId };
            var expectedException = new EnemyNotFoundException(enemy.Id);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((EnemyNotFoundException)exception).EnemyId.Should().Be(expectedException.EnemyId);
        }

        private readonly PrepareBattleHandler _commandHandler;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapRepository> _mapRepository;
        private readonly Mock<IPlayerRepository> _playerRepository;
        private readonly Mock<IClock> _clock;
        private readonly Mock<IBattleRepository> _battleRepository;
        private readonly IEnemyIncreaseStatsManager _enemyIncreaseStatsManager;
        private readonly Mock<IEnemyRepository> _enemyRepository;

        public PrepareBattleHandlerTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _mapRepository = new Mock<IMapRepository>();
            _playerRepository = new Mock<IPlayerRepository>();
            _clock = new Mock<IClock>();
            _clock.Setup(c => c.CurrentDate()).Returns(new DateTime(2022, 9, 3, 12, 30, 15));
            _battleRepository = new Mock<IBattleRepository>();
            _enemyIncreaseStatsManager = new EnemyIncreaseStatsManager(LoggerStub<EnemyIncreaseStatsManager>.Create());
            _enemyRepository = new Mock<IEnemyRepository>();
            _commandHandler = new PrepareBattleHandler(_userRepository.Object, _mapRepository.Object, _playerRepository.Object,
                _clock.Object, _battleRepository.Object, _enemyIncreaseStatsManager, _enemyRepository.Object);
        }
    }
}
