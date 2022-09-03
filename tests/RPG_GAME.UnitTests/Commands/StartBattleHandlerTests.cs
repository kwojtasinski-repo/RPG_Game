using FluentAssertions;
using Moq;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Commands
{
    public class StartBattleHandlerTests
    {
        [Fact]
        public async Task should_start_battle()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var hero = EntitiesFixture.CreateDefaultHero();
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            _playerRepository.Setup(p => p.GetByUserId(userId)).ReturnsAsync(player);
            var battle = BattleFixture.CreateBattleAtPrepare(DateTime.UtcNow, userId, map, player);
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            var command = new StartBattle { BattleId = battle.Id, UserId = userId };

            var battleSatus = await _commandHandler.HandleAsync(command);

            battleSatus.Should().NotBeNull();
            battleSatus.BattleId.Should().Be(battle.Id);
            battleSatus.EnemyId.Should().Be(enemy.Id);
            battleSatus.EnemyHealth.Should().Be(enemy.BaseHealth.Value);
            battleSatus.PlayerId.Should().Be(player.Id);
            battleSatus.PlayerHealth.Should().Be(player.Hero.Health);
            _battleRepository.Verify(b => b.UpdateAsync(It.IsAny<Battle>()), times: Times.Once);
        }

        [Fact]
        public async Task given_not_existing_player_when_start_battle_should_throw_an_exception()
        {
            var battleId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new StartBattle { BattleId = battleId, UserId = userId };
            var expectedException = new UserNotFoundException(userId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((UserNotFoundException)exception).UserId.Should().Be(expectedException.UserId);
        }

        [Fact]
        public async Task given_not_existing_battle_when_start_battle_should_throw_an_exception()
        {
            var battleId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var command = new StartBattle { BattleId = battleId, UserId = userId };
            var expectedException = new BattleNotFoundException(battleId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((BattleNotFoundException)exception).BattleId.Should().Be(expectedException.BattleId);
        }

        [Fact]
        public async Task given_battle_with_invalid_battle_info_when_start_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            var battle = BattleFixture.CreateBattleInProgress(DateTime.UtcNow, userId, map, player);
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var command = new StartBattle { BattleId = battle.Id, UserId = userId };
            var expectedException = new CannotStartBattleForBattleInfoException(battle.BattleInfo);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotStartBattleForBattleInfoException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
        }

        [Fact]
        public async Task given_battle_with_different_user_id_when_start_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(DateTime.UtcNow, player.UserId, map, player);
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var command = new StartBattle { BattleId = battle.Id, UserId = userId };
            var expectedException = new CannotStartBattleForUserException(battle.Id, command.UserId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotStartBattleForUserException)exception).BattleId.Should().Be(expectedException.BattleId);
            ((CannotStartBattleForUserException)exception).UserId.Should().Be(expectedException.UserId);
        }

        [Fact]
        public async Task given_invalid_player_when_start_battle_should_throw_an_exception()
        {
            var userId = Guid.NewGuid();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, userId);
            var battle = BattleFixture.CreateBattleAtPrepare(DateTime.UtcNow, player.UserId, map, player);
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            _userRepository.Setup(u => u.ExistsAsync(userId)).ReturnsAsync(true);
            var command = new StartBattle { BattleId = battle.Id, UserId = userId };
            var expectedException = new PlayerForUserNotFoundException(command.UserId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((PlayerForUserNotFoundException)exception).UserId.Should().Be(expectedException.UserId);
        }

        private readonly StartBattleHandler _commandHandler;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IPlayerRepository> _playerRepository;
        private readonly Mock<IBattleRepository> _battleRepository;
        private readonly Mock<IClock> _clock;

        public StartBattleHandlerTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _playerRepository = new Mock<IPlayerRepository>();
            _battleRepository = new Mock<IBattleRepository>();
            _clock = new Mock<IClock>();
            _clock.Setup(c => c.CurrentDate()).Returns(new DateTime(2022, 9, 3, 12, 30, 15));
            _commandHandler = new StartBattleHandler(_userRepository.Object, _battleRepository.Object, _playerRepository.Object, _clock.Object);
        }
    }
}
