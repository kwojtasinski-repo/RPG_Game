using Xunit;

namespace RPG_Game.IntegrationTests
{
    [CollectionDefinition("DbAndMappings")]
    public sealed class TestDbAndMappingCollectionFixture : ICollectionFixture<MongoDbTestFixture>, ICollectionFixture<MappingFixture>
    {
    }
}
