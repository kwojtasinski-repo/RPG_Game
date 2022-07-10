using RPG_Game.Infrastructure.Mappings;

namespace RPG_Game.Infrastructure
{
    public static class Extensions
    {
        public static void RegisterMappings()
        {
            MongoDbClassMap.RegisterAllMappings(typeof(Extensions).Assembly);
        }
    }
}