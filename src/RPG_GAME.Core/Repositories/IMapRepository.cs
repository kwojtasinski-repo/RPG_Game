using RPG_GAME.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IMapRepository
    {
        Task Add(Map map);
        Task Update(Map map);
        Task<Map> Get(Guid id);
        Task<IEnumerable<Map>> GetAll();
    }
}
