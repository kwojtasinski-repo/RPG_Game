using RPG_GAME.Core.Entities.Maps;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IMapRepository
    {
        Task AddAsync(Map map);
        Task UpdateAsync(Map map);
        Task<Map> GetAsync(Guid id);
        Task<IEnumerable<Map>> GetAllAsync();
    }
}
