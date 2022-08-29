using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class EnemyRepositoryStub : IEnemyRepository
    {
        private readonly IList<Enemy> _enemies = new List<Enemy>();

        public Task AddAsync(Enemy enemy)
        {
            _enemies.Add(enemy);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _enemies.Remove(_enemies.Single(e => e.Id == id));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Enemy>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Enemy>>(_enemies);
        }

        public Task<Enemy> GetAsync(Guid id)
        {
            return Task.FromResult(_enemies.SingleOrDefault(e => e.Id == id));
        }

        public async Task<IEnumerable<Enemy>> GetByMapIdAsync(Guid mapId)
        {
            await Task.CompletedTask;
            var enemies = _enemies
               .Where(e => e.MapsAssignedTo.Contains(mapId)).ToList();
            return enemies;
        }

        public Task UpdateAsync(Enemy enemy)
        {
            return Task.CompletedTask;
        }
    }
}
