using MongoDB.Bson.Serialization;
using RPG_GAME.Infrastructure.Mappings;
using RPG_GAME.Core.Common;

namespace RPG_GAME.Infrastructure.Configurations
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
