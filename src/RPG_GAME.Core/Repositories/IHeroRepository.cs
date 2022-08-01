using RPG_GAME.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IHeroRepository
    {
        Task AddAsync(Hero hero);
        Task UpdateAsync(Hero hero);
        Task DeleteAsync(Guid id);
        Task<Hero> GetAsync(Guid id);
        Task<IEnumerable<Hero>> GetAllAsync();
    }
}
