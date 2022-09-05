using Xunit;

namespace RPG_GAME.IntegrationTests.Common
{
    [CollectionDefinition("TestCollection")]
    public class SharedClassFixture : ICollectionFixture<TestApplicationFactory<Program>>
    {
    }
}
