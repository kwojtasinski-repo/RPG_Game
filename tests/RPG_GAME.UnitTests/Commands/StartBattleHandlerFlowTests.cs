using FluentAssertions;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using RPG_GAME.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Commands
{
    public class StartBattleHandlerFlowTests
    {
        [Fact]
        public async Task should_start_battle()
        {
            var user = await AddDefaultUser();
            var player = await AddDefaultPlayer(user.Id);
            var battle = await AddDefaultBattle(player, user.Id);
            var command = new StartBattle { BattleId = battle.Id, UserId = user.Id };

            var battleStatus = await _handler.HandleAsync(command);

            battleStatus.Should().NotBeNull();
            battleStatus.PlayerId.Should().Be(player.Id);
        }

        [Fact]
        public async Task should_start_battle_and_update_battle()
        {
            var user = await AddDefaultUser();
            var player = await AddDefaultPlayer(user.Id);
            var battle = await AddDefaultBattle(player, user.Id);
            var command = new StartBattle { BattleId = battle.Id, UserId = user.Id };

            var battleStatus = await _handler.HandleAsync(command);

            var battleUpdated = await _battleRepository.GetAsync(battleStatus.BattleId);
            battleUpdated.Should().NotBeNull();
            battleUpdated.BattleInfo.Should().Be(BattleInfo.InProgress);
            battleUpdated.BattleStates.Should().NotBeEmpty();
            battleUpdated.BattleStates.Should().HaveCount(2);
        }

        private async Task<Battle> AddDefaultBattle(Player player, Guid userId)
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = BattleFixture.CreateBattleAtPrepare(DateTime.UtcNow, userId, map, player);
            await _battleRepository.AddAsync(battle);
            return battle;
        }

        private async Task<Player> AddDefaultPlayer(Guid userId)
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero().AsAssign(), userId);
            await _playerRepository.AddAsync(player);
            return player;
        }

        private async Task<User> AddDefaultUser()
        {
            var user = EntitiesFixture.CreateDefaultUser("test@email.com", "pepe");
            await _userRepository.AddAsync(user);
            return user;
        }

        private readonly StartBattleHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IBattleRepository _battleRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IClock _clock;

        public StartBattleHandlerFlowTests()
        {
            _userRepository = new UserRepositoryStub();
            _battleRepository = new BattleRepositoryStub();
            _playerRepository = new PlayerRepositoryStub();
            _clock = new ClockStub();
            _handler = new StartBattleHandler(_userRepository, _battleRepository, _playerRepository, _clock);
        }
    }
}
