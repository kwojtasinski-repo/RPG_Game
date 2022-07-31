using RPG_Game.Infrastructure;

namespace RPG_Game.IntegrationTests
{
    public sealed class MappingFixture
    {
        public MappingFixture()
        {
            Extensions.RegisterMappings();
        }
    }
}
