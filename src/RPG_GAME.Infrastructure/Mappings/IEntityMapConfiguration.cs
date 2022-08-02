using MongoDB.Bson.Serialization;

namespace RPG_GAME.Infrastructure.Mappings
{
    public interface IEntityMapConfiguration<T>
        where T : class
    {
        public void Map(BsonClassMap<T> bsonClassMap);
    }
}
