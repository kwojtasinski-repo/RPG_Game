using MongoDB.Bson.Serialization;
using RPG_Game.Infrastructure.Mappings;
using RPG_GAME.Core.Common;

namespace RPG_Game.Infrastructure.Configurations
{
    internal class BaseEntityConfiguration : IEntityMapConfiguration<BaseEntity>
    {
        public void Map(BsonClassMap<BaseEntity> bsonClassMap)
        {
            bsonClassMap.SetIsRootClass(true);
            bsonClassMap.MapIdField(be => be.Id);
        }
    }
}
