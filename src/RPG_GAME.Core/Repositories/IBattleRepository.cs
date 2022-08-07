using RPG_GAME.Core.Entities.Battles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IBattleRepository
    {
        Task AddAsync(Battle battle);
        Task UpdateAsync(Battle battle);
        Task<Battle> GetAsync(Guid id);
        Task<IEnumerable<Battle>> GetByUserIdAsync(Guid userId);
    }
}
