using RPG_GAME.Core.Entities.Enemies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IEnemyRepository
    {
        Task AddAsync(Enemy enemy);
        Task UpdateAsync(Enemy enemy);
        Task<Enemy> GetAsync(Guid id);
        Task<IEnumerable<Enemy>> GetAllAsync();
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Enemy>> GetByMapIdAsync(Guid id);
    }
}
