using FluentAssertions;
using RPG_GAME.Application.Commands.Battles;
using RPG_GAME.Application.Commands.Battles.Handlers;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;
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
    public class PrepareBattleHandlerFlowTests
    {
        [Fact]
        public async Task should_prepare_battle()
        {
            var user = await AddDefaultUser();
            await AddDefaultPlayer(user.Id);
            var enemy = await AddDefaultEnemy();
            var map = await AddDefaultMap(new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var command = new PrepareBattle { MapId = map.Id, UserId = user.Id };

            var battleDetails = await _handler.HandleAsync(command);

            battleDetails.Should().NotBeNull();
            battleDetails.Map.Id.Should().Be(map.Id);
        }

        [Fact]
        public async Task should_prepare_battle_and_create_battle()
        {
            var user = await AddDefaultUser();
            await AddDefaultPlayer(user.Id);
            var enemy = await AddDefaultEnemy();
            var map = await AddDefaultMap(new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var command = new PrepareBattle { MapId = map.Id, UserId = user.Id };

            var battleDetails = await _handler.HandleAsync(command);

            var battle = await _battleRepository.GetAsync(battleDetails.Id);
            battle.Should().NotBeNull();
            battle.Map.Id.Should().Be(map.Id);
            battle.BattleInfo.Should().Be(BattleInfo.Starting);
            battle.BattleStates.Should().NotBeEmpty();
            battle.BattleStates.Should().HaveCount(1);
        }

        [Fact]
        public async Task should_create_prepare_battle_with_calculated_enemies()
        {
            var enemy = await AddDefaultEnemy();
            var map = await AddDefaultMap(new List<Enemies> { new Enemies(enemy.AsAssign(), 2) });
            var user = await AddDefaultUser();
            var player = await AddDefaultPlayer(user.Id, 5);
            var command = new PrepareBattle { MapId = map.Id, UserId = user.Id };
            var calculatedAttack = 104;
            var calculatedHealth = 104;
            var calculatedExperience = 1004;

            var battleDetails = await _handler.HandleAsync(command);

            var battle = await _battleRepository.GetAsync(battleDetails.Id);
            var enemyCalculated = battle.Map.Enemies.Single(e => e.Enemy.Id == enemy.Id);
            enemyCalculated.Enemy.Attack.Should().Be(calculatedAttack);
            enemyCalculated.Enemy.Health.Should().Be(calculatedHealth);
            enemyCalculated.Enemy.Experience.Should().Be(calculatedExperience);
        }

        private async Task<Enemy> AddDefaultEnemy()
        {
            var enemy = EntitiesFixture.CreateDefaultEnemy();
            await _enemyRepository.AddAsync(enemy);
            return enemy;
        }

        private async Task<Map> AddDefaultMap(IEnumerable<Enemies> enemies)
        {
            var map = EntitiesFixture.CreateDefaultMap(enemies: enemies);
            await _mapRepository.AddAsync(map);
            return map;
        }

        private async Task<Player> AddDefaultPlayer(Guid userId)
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero().AsAssign(), userId);
            await _playerRepository.AddAsync(player);
            return player;
        }

        private async Task<Player> AddDefaultPlayer(Guid userId, int level)
        {
            var player = new Player(Guid.NewGuid(), "player", EntitiesFixture.CreateDefaultHero().AsAssign(), level, 0, 5000, userId);
            await _playerRepository.AddAsync(player);
            return player;
        }

        private async Task<User> AddDefaultUser()
        {
            var user = new User(Guid.NewGuid(), "test@test.com", "password", "user", DateTime.UtcNow);
            await _userRepository.AddAsync(user);
            return user;
        }

        private readonly PrepareBattleHandler _handler;
        private readonly IUserRepository _userRepository;
        private readonly IMapRepository _mapRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IClock _clock;
        private readonly IBattleRepository _battleRepository;
        private readonly IEnemyIncreaseStatsManager _enemyIncreaseStatsManager;
        private readonly IEnemyRepository _enemyRepository;

        public PrepareBattleHandlerFlowTests()
        {
            _userRepository = new UserRepositoryStub();
            _mapRepository = new MapRepositoryStub();
            _playerRepository = new PlayerRepositoryStub();
            _clock = new ClockStub();
            _battleRepository = new BattleRepositoryStub();
            _enemyIncreaseStatsManager = new EnemyIncreaseStatsManager(LoggerStub<EnemyIncreaseStatsManager>.Create());
            _enemyRepository = new EnemyRepositoryStub();
            _handler = new PrepareBattleHandler(_userRepository, _mapRepository, _playerRepository,
                _clock, _battleRepository, _enemyIncreaseStatsManager, _enemyRepository);
        }
    }
}
