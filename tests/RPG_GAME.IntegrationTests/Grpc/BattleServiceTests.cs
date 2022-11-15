using Grpc.Core;
using Humanizer;
using Microsoft.AspNetCore.Http;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Maps;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;
using RPG_GAME.IntegrationTests.Common;
using RPG_GAME.IntegrationTests.Common.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.IntegrationTests.Grpc
{
    public class BattleServiceTests : GrpcTestBase
    {
        public BattleServiceTests(TestApplicationFactory<Program> fixture)
              : base(fixture)
        {
        }

        [Fact]
        public async Task given_not_existing_user_should_throw_an_exception()
        {
            // Arrange
            var client = new Common.Protos.Battle.BattleClient(Channel);
            var userId = Guid.NewGuid();
            var exceptionThrown = new UserNotFoundException(userId);
            var expectedException = new RpcException(new Status(StatusCode.FailedPrecondition, exceptionThrown.Message));
            var task = client.PrepareBattleAsync(new PrepareBattleRequest { MapId = Guid.NewGuid().ToString(), UserId = userId.ToString() });

            // Act
            var exception = await Record.ExceptionAsync(() => task.ResponseAsync);
            var headers = await task.ResponseHeadersAsync;

            // Assert
            Assert.NotNull(exception);
            Assert.IsType(expectedException.GetType(), exception);
            Assert.Contains($"{StatusCodes.Status400BadRequest}", ((RpcException)exception).Status.Detail);
            Assert.NotNull(headers);
            Assert.NotEmpty(headers);
            Assert.Contains(headers.First().Key, typeof(UserNotFoundException).Name.Underscore());
            Assert.Contains(headers.First().Value, expectedException.Message);
        }

        [Fact]
        public async Task given_not_existing_map_should_throw_an_exception()
        {
            // Arrange
            var client = new Common.Protos.Battle.BattleClient(Channel);
            var userId = new Guid("00000000-0000-0000-0000-000000000001");
            var mapId = Guid.NewGuid();
            var exceptionThrown = new MapNotFoundException(mapId);
            var expectedException = new RpcException(new Status(StatusCode.FailedPrecondition, exceptionThrown.Message));
            var task = client.PrepareBattleAsync(new PrepareBattleRequest { MapId = mapId.ToString(), UserId = userId.ToString() });

            // Act
            var exception = await Record.ExceptionAsync(() => task.ResponseAsync);
            var headers = await task.ResponseHeadersAsync;

            // Assert
            Assert.NotNull(exception);
            Assert.IsType(expectedException.GetType(), exception);
            Assert.Contains($"{StatusCodes.Status400BadRequest}", ((RpcException)exception).Status.Detail);
            Assert.NotNull(headers);
            Assert.NotEmpty(headers);
            Assert.Contains(headers.First().Key, typeof(MapNotFoundException).Name.Underscore());
            Assert.Contains(headers.First().Value, expectedException.Message);
        }

        [Fact]
        public async Task should_prepare_battle()
        {
            // Arrange
            var client = new Common.Protos.Battle.BattleClient(Channel);
            var userId = new Guid("00000000-0000-0000-0000-000000000002");
            var mapId = new Guid("00000000-0000-0000-0000-000000000004");
            var repository = GetService<IBattleRepository>();

            // Act
            var response = await client.PrepareBattleAsync(new PrepareBattleRequest { MapId = mapId.ToString(), UserId = userId.ToString() });

            // Assert
            var id = Guid.Parse(response.Id);
            Assert.NotEqual(Guid.Empty, id);
            Assert.Equal(userId, Guid.Parse(response.UserId));
            Assert.Equal(mapId, Guid.Parse(response.Map.Id));
            var battle = await repository.GetAsync(id);
            Assert.NotNull(battle);
            Assert.Equal(userId, battle.UserId);
            Assert.Equal(mapId, battle.Map.Id);
            Assert.Equal(BattleInfo.Starting, battle.BattleInfo);
            Assert.True(battle.BattleStates.Count() == 1);
        }

        [Fact]
        public async Task should_start_battle()
        {
            // Arrange
            var client = new Common.Protos.Battle.BattleClient(Channel);
            var userId = new Guid("00000000-0000-0000-0000-000000000002");
            var battleId = new Guid("00000000-0000-0000-0000-000000000001");
            var repository = GetService<IBattleRepository>();

            // Act
            var response = await client.StartBattleAsync(new BattleRequest { BattleId = battleId.ToString(), UserId = userId.ToString() });

            // Assert
            var id = Guid.Parse(response.BattleId);
            Assert.NotEqual(Guid.Empty, id);
            Assert.True(response.EnemyHealth > 0);
            Assert.True(response.PlayerHealth > 0);
            var battle = await repository.GetAsync(id);
            Assert.NotNull(battle);
            Assert.Equal(BattleInfo.InProgress, battle.BattleInfo);
            Assert.True(battle.BattleStates.Count() == 2);
        }

        [Fact]
        public async Task should_complete_battle_and_lost_fight()
        {
            // Arrange
            var client = new Common.Protos.Battle.BattleClient(Channel);
            var userId = new Guid("00000000-0000-0000-0000-000000000002");
            var battleId = new Guid("00000000-0000-0000-0000-000000000002");
            var repository = GetService<IBattleRepository>();

            // Act
            var response = await client.CompleteBattleAsync(new BattleRequest { BattleId = battleId.ToString(), UserId = userId.ToString() });

            // Assert
            var id = Guid.Parse(response.Id);
            Assert.NotEqual(Guid.Empty, id);
            var battle = await repository.GetAsync(id);
            Assert.NotNull(battle);
            Assert.Equal(BattleInfo.Lost, battle.BattleInfo);
            Assert.True(battle.BattleStates.Count() == 3);
        }

        [Fact]
        public async Task should_complete_battle_and_won_fight()
        {
            // Arrange
            var client = new Common.Protos.Battle.BattleClient(Channel);
            var userId = new Guid("00000000-0000-0000-0000-000000000002");
            var battleId = new Guid("00000000-0000-0000-0000-000000000003");
            var repository = GetService<IBattleRepository>();

            // Act
            var response = await client.CompleteBattleAsync(new BattleRequest { BattleId = battleId.ToString(), UserId = userId.ToString() });

            // Assert
            var id = Guid.Parse(response.Id);
            Assert.NotEqual(Guid.Empty, id);
            var battle = await repository.GetAsync(id);
            Assert.NotNull(battle);
            Assert.Equal(BattleInfo.Won, battle.BattleInfo);
            Assert.True(battle.BattleStates.Count() == 3);
        }

        [Fact]
        public async Task should_add_event_battle()
        {
            // Arrange
            var client = new Common.Protos.Battle.BattleClient(Channel);
            var enemyId = new Guid("00000000-0000-0000-0000-000000000001");
            var battleId = new Guid("00000000-0000-0000-0000-000000000004");
            var playerId = new Guid("00000000-0000-0000-0000-000000000001");
            var repository = GetService<IBattleRepository>();
            var currentBattleStateRepository = GetService<ICurrentBattleStateRepository>();
            var battleEventRepository = GetService<IBattleEventRepository>();

            // Act
            var response = await client.AddBattleEventAsync(new AddBattleEventRequest { BattleId = battleId.ToString(), Action = "attack", EnemyId = enemyId.ToString(), PlayerId = playerId.ToString() });

            // Assert
            var id = Guid.Parse(response.Id);
            Assert.NotEqual(Guid.Empty, id);
            Assert.True(response.Level != default);
            Assert.True(response.CurrentExp != default);
            Assert.True(response.RequiredExp != default);
            var battle = await repository.GetAsync(battleId);
            Assert.NotNull(battle);
            var battleEvent = await battleEventRepository.GetAsync(id);
            Assert.NotNull(battleEvent);
            var currentBattleState = await currentBattleStateRepository.GetByBattleIdAsync(battleId);
            Assert.NotNull(currentBattleState);
        }

        [Fact]
        public async Task should_add_battle_events_till_won_fight()
        {
            // Arrange
            var enemyId = new Guid("00000000-0000-0000-0000-000000000001");
            var battleId = new Guid("00000000-0000-0000-0000-000000000005");
            var playerId = new Guid("00000000-0000-0000-0000-000000000001");
            var mapId = new Guid("00000000-0000-0000-0000-000000000005");
            var repository = GetService<IBattleRepository>();
            var currentBattleStateRepository = GetService<ICurrentBattleStateRepository>();
            var battleEventRepository = GetService<IBattleEventRepository>();
            var mapRepository = GetService<IMapRepository>();
            var map = await mapRepository.GetAsync(mapId);
            
            // Act
            var response = await FightTillEnemyOrPlayerKilled(new AddBattleEventRequest { BattleId = battleId.ToString(), Action = "attack", EnemyId = enemyId.ToString(), PlayerId = playerId.ToString() },
                100, map, 3);

            // Assert
            var id = Guid.Parse(response.Id);
            Assert.NotEqual(Guid.Empty, id);
            Assert.True(response.Level > 1);
            var battle = await repository.GetAsync(battleId);
            Assert.Equal(BattleInfo.Won, battle.BattleInfo);
            Assert.True(battle.BattleStates.Count() == 3);
            Assert.NotNull(battle);
            var battleEvent = await battleEventRepository.GetByBattleIdAsync(battleId);
            Assert.NotNull(battleEvent);
            Assert.NotEmpty(battleEvent);
            var currentBattleState = await currentBattleStateRepository.GetByBattleIdAsync(battleId);
            Assert.NotNull(currentBattleState);
            Assert.True(currentBattleState.EnemyHealth <= 0);
        }

        private async Task<AddBattleEventResponse> FightTillEnemyOrPlayerKilled(AddBattleEventRequest command, int playerHealthStarted, Core.Entities.Maps.Map map, int enemiesToKilled)
        {
            AddBattleEventResponse battleEvent = null;
            var playerHealth = playerHealthStarted;
            var enemyHealth = 0;
            var enemiesKilled = new List<string>();
            var enemiesCount = enemiesToKilled;
            var enemyId = command.EnemyId;
            var client = new Common.Protos.Battle.BattleClient(Channel);

            do
            {
                battleEvent = await client.AddBattleEventAsync(command);
                enemyHealth = battleEvent.Action.Health;
                playerHealth -= battleEvent.Action.DamageDealt;

                if (enemyHealth <= 0)
                {
                    enemiesKilled.Add(enemyId);
                    enemiesCount--;
                    var enemyToKilledCount = map.Enemies.SingleOrDefault(e => e.Enemy.Id.ToString() == enemyId).Quantity;
                    enemyId = GetNextEnemyId(enemyToKilledCount, enemyId, enemiesKilled);
                }
            }
            while (enemiesCount > 0 && playerHealth > 0);

            string GetNextEnemyId(int enemyToKilledCount, string currentEnemy, IEnumerable<string> enemiesKilled)
            {
                var nextEnemy = GetNextEnemy(enemiesKilled);
                return enemyToKilledCount > enemiesKilled.Where(e => e == currentEnemy).Count()
                        ? enemyId : nextEnemy != null ? nextEnemy.Enemy.Id.ToString() : currentEnemy;
            }

            Core.Entities.Maps.Enemies GetNextEnemy(IEnumerable<string> enemiesKilled)
            {
                return map.Enemies.SingleOrDefault(e => !enemiesKilled.Contains(e.Enemy.Id.ToString()));
            }

            return battleEvent;
        }
    }
}
