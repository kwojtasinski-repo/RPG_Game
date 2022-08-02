using MongoDB.Bson.Serialization;
using RPG_GAME.Infrastructure.Mappings;
using RPG_GAME.Core.ValueObjects;

namespace RPG_GAME.Infrastructure.Configurations
{
    internal sealed class EmailConfiguration : IEntityMapConfiguration<Email>
    {
        public void Map(BsonClassMap<Email> bsonClassMap)
        {
            bsonClassMap.MapField("_email").SetElementName("email");
        }
    }
}
