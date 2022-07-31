using MongoDB.Bson.Serialization;
using RPG_Game.Infrastructure.Mappings;
using RPG_GAME.Core.Entities;

namespace RPG_Game.Infrastructure.Configurations
{
    internal sealed class UserConfiguration : IEntityMapConfiguration<User>
    {
        public void Map(BsonClassMap<User> bsonClassMap)
        {
            bsonClassMap.MapProperty(u => u.Id);
            bsonClassMap.MapProperty(u => u.Email).SetElementName("email");
            bsonClassMap.MapProperty(u => u.Password).SetElementName("password");
        }
    }
}
