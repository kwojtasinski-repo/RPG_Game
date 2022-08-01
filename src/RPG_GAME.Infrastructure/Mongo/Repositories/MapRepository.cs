using MongoDB.Driver;
using RPG_Game.Infrastructure.Mongo.Documents;
using RPG_Game.Infrastructure.Mongo.Mappings;
using RPG_GAME.Core.Entities;
using RPG_GAME.Core.Repositories;

namespace RPG_Game.Infrastructure.Mongo.Repositories
{
    internal class MapRepository : IMapRepository
    {
        private readonly IMongoRepository<MapDocument, Guid> _repository;

        public MapRepository(IMongoRepository<MapDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task Add(Map map)
        {
            var document = map.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<Map> Get(Guid id)
        {
            var map = await _repository.GetAsync(id);
            return map.AsEntity();
        }

        public async Task<IEnumerable<Map>> GetAll()
        {
            var maps = await _repository.Collection.AsQueryable().ToListAsync();
            return maps.Select(e => e.AsEntity());
        }

        public async Task Update(Map map)
        {
            var document = map.AsDocument();
            await _repository.UpdateAsync(document);
        }
    }
}
