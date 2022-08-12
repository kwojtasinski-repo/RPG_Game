using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class MapRepositoryStub : IMapRepository
    {
        private readonly IList<Map> _maps = new List<Map>();

        public Task AddAsync(Map map)
        {
            _maps.Add(map);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Map>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Map>>(_maps);
        }

        public Task<IEnumerable<Map>> GetAllMapsByEnemyId(Guid enemyId)
        {
            var maps = _maps.Where(m => m.Enemies.Any(en => en.Enemy.Id == enemyId));
            return Task.FromResult(maps);
        }

        public Task<Map> GetAsync(Guid id)
        {
            return Task.FromResult(_maps.SingleOrDefault(m => m.Id == id));
        }

        public Task UpdateAsync(Map map)
        {
            return Task.CompletedTask;
        }
    }
}
