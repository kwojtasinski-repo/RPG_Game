using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using RPG_Game.Infrastructure;
using RPG_GAME.Core.Entity;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RPG_Game.IntegrationTests
{
    public class MappingTests
    {
        [Fact]
        public async Task should_add_document_to_database()
        {
            Extensions.RegisterMappings();
            var database = _mongoClient.GetDatabase(DatabaseName);
            var collection = database.GetCollection<Enemy>(CollectionName);
            var enemy = new Enemy(1, "Enemy #1", 10, 100, "Hard", 3);

            await collection.InsertOneAsync(enemy);

            var enemyFromDb = await collection.Find(e => e.Id == enemy.Id).SingleOrDefaultAsync();
            enemyFromDb.ShouldNotBeNull();
            enemyFromDb.Name.ShouldBe(enemy.Name);
        }

        private const string ConnectionString = "mongodb://localhost:27017";
        private const string DatabaseName = "test-rpg-game";
        private const string CollectionName = "enemies";
        private readonly IMongoClient _mongoClient;

        public MappingTests()
        {
            _mongoClient = new MongoClient(ConnectionString);
        }
    }
}