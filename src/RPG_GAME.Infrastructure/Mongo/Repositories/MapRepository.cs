﻿using MongoDB.Driver;
using RPG_GAME.Infrastructure.Mongo.Documents.Maps;
using RPG_GAME.Infrastructure.Mongo.Mappings;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal class MapRepository : IMapRepository
    {
        private readonly IMongoRepository<MapDocument, Guid> _repository;

        public MapRepository(IMongoRepository<MapDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Map map)
        {
            var document = map.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<Map> GetAsync(Guid id)
        {
            var map = await _repository.GetAsync(id);
            return map.AsEntity();
        }

        public async Task<IEnumerable<Map>> GetAllAsync()
        {
            var maps = await _repository.Collection.AsQueryable().ToListAsync();
            return maps.Select(e => e.AsEntity());
        }

        public async Task UpdateAsync(Map map)
        {
            var document = map.AsDocument();
            await _repository.UpdateAsync(document);
        }
    }
}
