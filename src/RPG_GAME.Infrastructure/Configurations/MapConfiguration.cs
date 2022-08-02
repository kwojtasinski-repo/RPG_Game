using MongoDB.Bson.Serialization;
using RPG_GAME.Infrastructure.Mappings;
using RPG_GAME.Core.Entities.Maps;

namespace RPG_GAME.Infrastructure.Configurations
{
    internal sealed class MapConfiguration : IEntityMapConfiguration<Map>
    {
        public void Map(BsonClassMap<Map> bsonClassMap)
        {
            bsonClassMap.AutoMap();
        }
    }
}
