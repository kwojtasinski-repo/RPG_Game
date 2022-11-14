using FluentAssertions;
using Moq;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Exceptions.Players;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Battles.Actions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Commands
{
    public class AddBattleEventHandlerTests
    {
        [Fact]
        public async Task should_add_battle_event()
        {
            var player = AddDefaultPlayer();
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = BattleFixture.CreateBattleInProgress(DateTime.Now, Guid.NewGuid(), map, player);
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            var command = new AddBattleEvent { BattleId = battle.Id, EnemyId = enemy.Id, PlayerId = player.Id, Action = "attack" };
            _battleManager.Setup(b => b.CreateBattleEvent(It.IsAny<Battle>(), enemy.Id, It.IsAny<Player>(), It.IsAny<string>())).ReturnsAsync(CreateDefaultBattleEvent(battle.Id));

            await _commandHandler.HandleAsync(command);

            _battleRepository.Verify(b => b.UpdateAsync(It.IsAny<Battle>()), times: Times.Once);
            _battleEventRepository.Verify(b => b.AddAsync(It.IsAny<BattleEvent>()), times: Times.Once);
        }

        [Fact]
        public async Task given_invalid_player_should_throw_an_exception()
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = BattleFixture.CreateBattleInProgress(DateTime.Now, Guid.NewGuid(), map, player);
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            var command = new AddBattleEvent { BattleId = battle.Id, EnemyId = enemy.Id, PlayerId = player.Id, Action = "attack" };
            var expectedException = new PlayerNotFoundException(player.Id);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((PlayerNotFoundException)exception).PlayerId.Should().Be(expectedException.PlayerId);
        }

        [Theory]
        [MemberData(nameof(GetBattleData))]
        public async Task given_battle_with_incorrect_battle_info_should_throw_an_exception(Battle battle)
        {
            _battleRepository.Setup(b => b.GetAsync(battle.Id)).ReturnsAsync(battle);
            var command = new AddBattleEvent { BattleId = battle.Id, EnemyId = Guid.NewGuid(), PlayerId = Guid.NewGuid(), Action = "attack" };
            var expectedException = new CannotAddBattleEventForBattleInfoException(battle.BattleInfo);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotAddBattleEventForBattleInfoException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
        }

        [Fact]
        public async Task given_invalid_battle_id_should_throw_an_exception()
        {
            var battleId = Guid.NewGuid();
            var command = new AddBattleEvent { BattleId = battleId, EnemyId = Guid.NewGuid(), PlayerId = Guid.NewGuid(), Action = "attack" };
            var expectedException = new BattleNotFoundException(battleId);

            var exception = await Record.ExceptionAsync(() => _commandHandler.HandleAsync(command));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((BattleNotFoundException)exception).BattleId.Should().Be(expectedException.BattleId);
        }

        public static TheoryData<Battle> GetBattleData
            => new()
            {
                CreateDefaultBattleWithStatus(BattleStatus.Prepare),
                CreateDefaultBattleWithStatus(BattleStatus.Completed)
            };

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

        private BattleEvent CreateDefaultBattleEvent(Guid battleId)
        {
            return new BattleEvent(Guid.NewGuid(), battleId, new FightAction(Guid.NewGuid(), CharacterType.ENEMY.ToString(), "name", 100, 100, "attack"), 1, 100, 1000, DateTime.UtcNow);
        }

        private Player AddDefaultPlayer()
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            _playerRepository.Setup(p => p.GetAsync(player.Id)).ReturnsAsync(player);
            return player;
        }

        private readonly AddBattleEventHandler _commandHandler;
        private readonly Mock<IBattleRepository> _battleRepository;
        private readonly Mock<IPlayerRepository> _playerRepository;
        private readonly Mock<IBattleManager> _battleManager;
        private readonly Mock<IBattleEventRepository> _battleEventRepository;

        public AddBattleEventHandlerTests()
        {
            _battleRepository = new Mock<IBattleRepository>();
            _playerRepository = new Mock<IPlayerRepository>();
            _battleManager = new Mock<IBattleManager>();
            _battleEventRepository = new Mock<IBattleEventRepository>();
            _commandHandler = new AddBattleEventHandler(_battleRepository.Object, _playerRepository.Object, _battleManager.Object, _battleEventRepository.Object);
        }
    }
}
