using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Net.Client;
using RPG_GAME.Protos;
using System.Linq;

namespace RPG_GAME
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            var baseUri = "http://localhost:50000";
            using var channel = GrpcChannel.ForAddress(baseUri);
            var client = new Battle.BattleClient(channel);
            var mapId = "2c70e345-024f-4b12-b1ff-11f33ff40fdb";
            var userId = "68945a59-bb5f-48d0-a855-4a8b679c8e74";

            var preparedBattle = await client.PrepareBattleAsync(new PrepareBattleRequest { MapId = mapId, UserId = userId });
            Console.WriteLine(preparedBattle.ToString());
            var battleId = preparedBattle.Id;
            var startedBattle = await client.StartBattleAsync(new BattleRequest { BattleId = battleId, UserId = userId });
            Console.WriteLine(startedBattle.ToString());
            var enemyId = startedBattle.EnemyId;
            var playerId = startedBattle.PlayerId;
            var response = await client.AddBattleEventAsync(new AddBattleEventRequest { BattleId = battleId, Action = "Cyclon", EnemyId = enemyId, PlayerId = playerId });
            Console.WriteLine(response.ToString());
            var playerHealth = startedBattle.PlayerHealth - response.Action.DamageDealt;
            var enemiesCount = preparedBattle.Map.Enemies.Sum(e => e.Quantity);
            var enemyHealth = response.Action.Health;
            var enemiesKilled = new List<string>();

            if (enemyHealth <= 0)
            {
                enemiesKilled.Add(enemyId);
                enemiesCount--;
                var enemyToKilledCount = preparedBattle.Map.Enemies.SingleOrDefault(e => e.Enemy.Id == enemyId).Quantity;
                var nextEnemy = preparedBattle.Map.Enemies.SingleOrDefault(e => !enemiesKilled.Contains(e.Enemy.Id));
                enemyId = enemyToKilledCount > enemiesKilled.Where(e => e == enemyId).Count()
                    ? enemyId : nextEnemy != null ? nextEnemy.Enemy.Id : enemyId;
            }

            while (enemiesCount > 0 && playerHealth > 0)
            {
                response = await client.AddBattleEventAsync(new AddBattleEventRequest { BattleId = battleId, Action = "Cyclon", EnemyId = enemyId, PlayerId = playerId });
                Console.WriteLine(response.ToString());
                enemyHealth = response.Action.Health;
                playerHealth -= response.Action.DamageDealt;
                Console.WriteLine();
                Console.WriteLine();

                if (enemyHealth <= 0)
                {
                    enemiesKilled.Add(enemyId);
                    enemiesCount--;
                    var enemyToKilledCount = preparedBattle.Map.Enemies.SingleOrDefault(e => e.Enemy.Id == enemyId).Quantity;
                    var nextEnemy = preparedBattle.Map.Enemies.SingleOrDefault(e => !enemiesKilled.Contains(e.Enemy.Id));
                    enemyId = enemyToKilledCount > enemiesKilled.Where(e => e == enemyId).Count()
                        ? enemyId : nextEnemy != null ? nextEnemy.Enemy.Id : enemyId;
                }
            }

            Console.WriteLine(response.ToString());
            if (enemyHealth <= 0)
            {
                Console.WriteLine("Battle Won!");
            }
            Console.WriteLine($"Player Health {playerHealth}");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }
}
