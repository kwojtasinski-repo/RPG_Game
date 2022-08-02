using MongoDB.Bson.Serialization;
using RPG_GAME.Infrastructure.Mappings;
using RPG_GAME.Core.Entity;

namespace RPG_GAME.Infrastructure.Configurations
{
    internal class EnemyConfiguration : IEntityMapConfiguration<Enemy>
    {
        public void Map(BsonClassMap<Enemy> bsonClassMap)
        {
            bsonClassMap.AutoMap();
            bsonClassMap.SetIdMember(bsonClassMap.GetMemberMap(c => c.Id));
        }
    }
}
