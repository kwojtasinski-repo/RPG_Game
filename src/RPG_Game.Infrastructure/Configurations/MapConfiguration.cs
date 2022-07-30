using MongoDB.Bson.Serialization;
using RPG_Game.Infrastructure.Mappings;
using RPG_GAME.Core.Entities;

namespace RPG_Game.Infrastructure.Configurations
{
    internal sealed class MapConfiguration : IEntityMapConfiguration<Map>
    {
        public void Map(BsonClassMap<Map> bsonClassMap)
        {
            bsonClassMap.AutoMap();
        }
    }
}
