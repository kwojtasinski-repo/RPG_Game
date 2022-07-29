using MongoDB.Bson.Serialization;

namespace RPG_Game.Infrastructure.Mappings
{
    public interface IEntityMapConfiguration<T>
        where T : class
    {
        public void Map(BsonClassMap<T> bsonClassMap);
    }
}
