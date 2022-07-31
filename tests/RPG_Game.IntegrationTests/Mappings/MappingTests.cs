using MongoDB.Driver;
using RPG_Game.Infrastructure;
using RPG_GAME.Core.Entity;
using RPG_GAME.Core.Entities;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Collections.Generic;
using RPG_GAME.Core.ValueObjects;
using MongoDB.Bson.Serialization;
using System.Linq;

namespace RPG_Game.IntegrationTests
{
    public class MappingTests
    {
        [Fact]
        public async Task should_add_document_to_database()
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<RPG_GAME.Core.Entity.Enemy>(CollectionName);
            var enemy = new RPG_GAME.Core.Entity.Enemy(1, "Enemy #1", 10, 100, "Hard", 3);

            await collection.InsertOneAsync(enemy);

            var enemyFromDb = await collection.Find(e => e.Id == enemy.Id).SingleOrDefaultAsync();
            enemyFromDb.ShouldNotBeNull();
            enemyFromDb.Name.ShouldBe(enemy.Name);
        }

        [Fact]
        public async Task should_map_and_add_map_to_database()
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<Map>("maps");
            var map = new Map { Id = Guid.NewGuid(), Difficulty = Difficulty.EASY, Name = "Map #1", Enemies = new List<RequiredEnemy> { new RequiredEnemy { EnemyId = Guid.NewGuid(), Quantity = 1 } } };

            await collection.InsertOneAsync(map);

            var mapFromDb = await collection.Find(m => m.Id == map.Id).SingleOrDefaultAsync();
            mapFromDb.ShouldNotBeNull();
            mapFromDb.Name.ShouldBe(map.Name);
        }

        [Fact]
        public async Task should_add_user_to_database()
        {
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<User>("users");
            var user = new User { Id = Guid.NewGuid(), Email = Email.From("test@test.com"), Password = "test" };

            await collection.InsertOneAsync(user);

            var mapFromDb = await collection.Find(m => m.Id == user.Id).SingleOrDefaultAsync();
            mapFromDb.ShouldNotBeNull();
            mapFromDb.Email.ShouldBe(user.Email);
        }

        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "test-rpg-game";
        private const string CollectionName = "enemies";
        private readonly IMongoClient _mongoClient;

        public MappingTests()
        {
            Extensions.RegisterMappings();
            _mongoClient = new MongoClient(ConnectionString);
        }
    }
}