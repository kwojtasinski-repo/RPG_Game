using FluentAssertions;
using Moq;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Exceptions.Battles;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Commands
{
    public class CompleteBattleHandlerTests
    {
        [Fact]
        public async Task should_complete_battle()
        {
            var user = AddDefaultUser();
            var battle = AddDefaultBattle(user.Id);
            var player = GetPlayerFromBattle(battle);
            _battleManager.Setup(b => b.CompleteBattle(It.IsAny<Battle>(), It.IsAny<Player>())).ReturnsAsync(player);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };

            var battleDetails = await _commandHandler.HandleAsync(command);

            _battleRepository.Verify(p => p.UpdateAsync(It.IsAny<Battle>()), times: Times.Once);
            _playerRepository.Verify(p => p.UpdateAsync(It.IsAny<Player>()), times: Times.Once);
            battleDetails.Id.Should().Be(battle.Id);
        }

        [Fact]
        public async Task given_admin_user_and_different_user_id_in_battle_should_complete_battle()
        {
            var user = AddDefaultUser("admin");
            var userId = Guid.NewGuid();
            var battle = AddDefaultBattle(userId);
            var player = GetPlayerFromBattle(battle);
            _battleManager.Setup(b => b.CompleteBattle(It.IsAny<Battle>(), It.IsAny<Player>())).ReturnsAsync(player);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };

            var battleDetails = await _commandHandler.HandleAsync(command);

            _battleRepository.Verify(p => p.UpdateAsync(It.IsAny<Battle>()), times: Times.Once);
            _playerRepository.Verify(p => p.UpdateAsync(It.IsAny<Player>()), times: Times.Once);
            battleDetails.Id.Should().Be(battle.Id);
        }

        [Fact]
        public async Task given_different_user_in_battle_should_throw_an_exception()
        {
            var user = AddDefaultUser();
            var userId = Guid.NewGuid();
            var battle = AddDefaultBattle(userId);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };
            var expectedException = new CannotCompleteBattleForUserException(battle.Id, user.Id);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotCompleteBattleForUserException)exception).BattleId.Should().Be(expectedException.BattleId);
            ((CannotCompleteBattleForUserException)exception).UserId.Should().Be(expectedException.UserId);
        }

        [Theory]
        [MemberData(nameof(GetBattleData))]
        public async Task given_invalid_battle_info_should_throw_an_exception(Battle battle)
        {
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            var user = AddDefaultUser(battle.UserId);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };
            var expectedException = new CannotGetBattleStateInActionException(battle.BattleInfo);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotGetBattleStateInActionException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
        }

        [Fact]
        public async Task given_invalid_battle_id_should_throw_an_exception()
        {
            var battle = CreateDefaultBattleWithStatus(BattleStatus.InAction);
            var user = AddDefaultUser(battle.UserId);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };
            var expectedException = new BattleNotFoundException(battle.Id);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((BattleNotFoundException)exception).BattleId.Should().Be(expectedException.BattleId);
        }

        [Fact]
        public async Task given_invalid_user_id_should_throw_an_exception()
        {
            var battleId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var command = new CompleteBattle { BattleId = battleId, UserId = userId };
            var expectedException = new UserNotFoundException(userId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((UserNotFoundException)exception).UserId.Should().Be(expectedException.UserId);
        }

        public static TheoryData<Battle> GetBattleData
            => new ()
            {
                CreateDefaultBattleWithStatus(BattleStatus.Prepare),
                CreateDefaultBattleWithStatus(BattleStatus.Completed)
            };

        private Player GetPlayerFromBattle(Battle battle)
        {
            var batttleState = battle.GetBattleStateInAction();
            return batttleState.Player;
        }

        private User AddDefaultUser(string role = null)
        {
            var user = EntitiesFixture.CreateDefaultUser("email@email.com", "password", role);
            _userRepository.Setup(u => u.GetAsync(user.Id)).ReturnsAsync(user);
            return user;
        }

        private User AddDefaultUser(Guid id, string role = null)
        {
            var user = new User(id, "email@email.com", "password", role ?? "user", DateTime.UtcNow);
            _userRepository.Setup(u => u.GetAsync(user.Id)).ReturnsAsync(user);
            return user;
        }

        private Battle AddDefaultBattle(Guid? userId = null)
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var created = DateTime.UtcNow;
            var battle = BattleFixture.CreateBattleInProgress(created, userId ?? Guid.NewGuid(), map, player);
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            return battle;
        }

        private static Battle CreateDefaultBattleWithStatus(BattleStatus battleStatus)
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var created = DateTime.UtcNow;
            var battle = battleStatus switch
            {
                BattleStatus.Prepare => BattleFixture.CreateBattleAtPrepare(created, Guid.NewGuid(), map, player),
                BattleStatus.InAction => BattleFixture.CreateBattleInProgress(created, Guid.NewGuid(), map, player),
                BattleStatus.Completed => BattleFixture.CreateBattleCompleted(created, Guid.NewGuid(), map, player, BattleInfo.Suspended),
                _ => BattleFixture.CreateBattleAtPrepare(created, Guid.NewGuid(), map, player)
            };
            return battle;
        }

        private readonly CompleteBattleHandler _commandHandler;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IBattleRepository> _battleRepository;
        private readonly Mock<IBattleManager> _battleManager;
        private readonly Mock<IPlayerRepository> _playerRepository;

        public CompleteBattleHandlerTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _battleRepository = new Mock<IBattleRepository>();
            _battleManager = new Mock<IBattleManager>();
            _playerRepository = new Mock<IPlayerRepository>();
            _commandHandler = new CompleteBattleHandler(_userRepository.Object, _battleRepository.Object, _battleManager.Object, _playerRepository.Object);
        }
    }
}
