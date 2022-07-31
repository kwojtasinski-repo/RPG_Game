using MongoDB.Driver;
using System;

namespace RPG_Game.IntegrationTests
{
    public sealed class MongoDbTestFixture : IDisposable
    {
        public MongoClient MongoClient;
        private const string ConnectionString = "mongodb://localhost:27017";
        public const string DatabaseName = "test-rpg-game";

        public MongoDbTestFixture()
        {
            MongoClient = new MongoClient(ConnectionString);
        }

        public void Dispose()
        {
            MongoClient.DropDatabase(DatabaseName);
        }
    }
}
