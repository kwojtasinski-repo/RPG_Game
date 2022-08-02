using Xunit;

namespace RPG_GAME.IntegrationTests
{
    [CollectionDefinition("DbAndMappings")]
    public sealed class TestDbAndMappingCollectionFixture : ICollectionFixture<MongoDbTestFixture>, ICollectionFixture<MappingFixture>
    {
    }
}
