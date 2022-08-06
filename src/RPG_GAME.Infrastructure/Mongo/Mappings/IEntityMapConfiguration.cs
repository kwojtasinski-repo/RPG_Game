using MongoDB.Driver;
using RPG_GAME.Infrastructure.Mongo.Documents;
using RPG_GAME.Infrastructure.Mongo.Repositories;

namespace RPG_GAME.Infrastructure.Mappings
{
    public interface IEntityMapConfiguration<T, TIdentifiable>
        where T : class, IIdentifiable<TIdentifiable>
    {
        public Task AddIndex(IMongoRepository<T, TIdentifiable> repo, IndexKeysDefinitionBuilder<T> indexKeysDefinitionBuilder);
    }
}
