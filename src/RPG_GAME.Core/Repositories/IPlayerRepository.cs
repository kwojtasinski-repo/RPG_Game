using RPG_GAME.Core.Entities.Players;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IPlayerRepository
    {
        Task Add(Player map);
        Task Update(Player map);
        Task<Player> Get(Guid id);
        Task<IEnumerable<Player>> GetAll();
    }
}
