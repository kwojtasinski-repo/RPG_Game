using MongoDB.Bson.Serialization;
using RPG_GAME.Infrastructure.Mappings;
using RPG_GAME.Core.Entities.Users;

namespace RPG_GAME.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityMapConfiguration<User>
    {
        public void Map(BsonClassMap<User> bsonClassMap)
        {
            bsonClassMap.MapProperty(u => u.Id);
            bsonClassMap.MapProperty(u => u.Email);
            bsonClassMap.MapProperty(u => u.Password);
        }
    }
}
