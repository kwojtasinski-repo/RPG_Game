using FluentAssertions;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.Managers;
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
    public class CompleteBattleHandlerFlowTests
    {
        [Fact]
        public async Task should_complete_battle()
        {
            var user = await AddDefaultUser();
            var player = await AddDefaultPlayer(user.Id);
            var battle = await AddDefaultBattle(player, user.Id);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };

            var battleDetails = await _handler.HandleAsync(command);

            battleDetails.Should().NotBeNull();
            battleDetails.EndDate.Should().NotBeNull();
            battleDetails.EnemiesKilled.Should().BeEmpty();
            battleDetails.BattleInfo.Should().Be(BattleInfo.Lost.ToString());
            battleDetails.BattleStates.Should().NotBeEmpty();
            battleDetails.BattleStates.Should().HaveCount(3);
        }

        [Fact]
        public async Task should_complete_battle_and_update_battle()
        {
            var user = await AddDefaultUser();
            var player = await AddDefaultPlayer(user.Id);
            var battle = await AddDefaultBattle(player, user.Id);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };

            var battleDetails = await _handler.HandleAsync(command);

            var battleUpdated = await _battleRepository.GetAsync(battleDetails.Id);
            battleUpdated.Should().NotBeNull();
            battleUpdated.EndDate.Should().NotBeNull();
            battleUpdated.EnemiesKilled.Should().BeEmpty();
            battleUpdated.BattleInfo.Should().Be(BattleInfo.Lost);
            battleUpdated.BattleStates.Should().NotBeEmpty();
            battleUpdated.BattleStates.Should().HaveCount(3);
        }

        [Fact]
        public async Task should_complete_battle_and_update_battle_as_won()
        {
            var user = await AddDefaultUser();
            var player = await AddDefaultPlayer(user.Id);
            var battle = await AddDefaultBattle(player, user.Id);
            AddAllEnemiesAsKilled(battle);
            var command = new CompleteBattle { BattleId = battle.Id, UserId = user.Id };

            var battleDetails = await _handler.HandleAsync(command);

            var battleUpdated = await _battleRepository.GetAsync(battleDetails.Id);
            battleUpdated.Should().NotBeNull();
            battleUpdated.EndDate.Should().NotBeNull();
            battleUpdated.EnemiesKilled.Should().NotBeEmpty();
            battleUpdated.BattleInfo.Should().Be(BattleInfo.Won);
            battleUpdated.BattleStates.Should().NotBeEmpty();
            battleUpdated.BattleStates.Should().HaveCount(3);
        }

        private void AddAllEnemiesAsKilled(Battle battle)
        {
            var enemies = battle.Map.Enemies;

            foreach(var enemy in enemies)
            {
                for (int i = 0; i < enemy.Quantity; i++)
                {
                    battle.AddKilledEnemy(enemy.Enemy.Id);
                }
            }
        }

        private async Task<Player> AddDefaultPlayer(Guid userId)
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero().AsAssign(), userId);
            await _playerRepository.AddAsync(player);
            return player;
        }

        private async Task<Battle> AddDefaultBattle(Player player, Guid userId)
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var battle = BattleFixture.CreateBattleInProgress(DateTime.UtcNow, userId, map, player);
            await _battleRepository.AddAsync(battle);
            return battle;
        }

        private async Task<User> AddDefaultUser(string role = null)
        {
            var user = EntitiesFixture.CreateDefaultUser("test@email.com", "pepe", role: role);
            await _userRepository.AddAsync(user);
            return user;
        }

        private readonly CompleteBattleHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IBattleRepository _battleRepository;
        private readonly IBattleManager _battleManager;
        private readonly IPlayerRepository _playerRepository;
        private readonly IClock _clock;

        public CompleteBattleHandlerFlowTests()
        {
            _userRepository = new UserRepositoryStub();
            _battleRepository = new BattleRepositoryStub();
            _playerRepository = new PlayerRepositoryStub();
            _clock = new ClockStub();
            _battleManager = new BattleManager(_clock, new CurrentBattleStateRepositoryStub(), new HeroRepositoryStub(), new EnemyRepositoryStub(),
                new EnemyAttackManager(), new PlayerIncreaseStatsManager(LoggerStub<PlayerIncreaseStatsManager>.Create()),
                new EnemyIncreaseStatsManager(LoggerStub<EnemyIncreaseStatsManager>.Create()));
            _handler = new CompleteBattleHandler(_userRepository, _battleRepository, _battleManager, _playerRepository);
        }
    }
}
