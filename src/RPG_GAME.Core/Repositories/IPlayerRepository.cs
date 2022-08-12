using RPG_GAME.Core.Entities.Players;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IPlayerRepository
    {
        Task AddAsync(Player player);
        Task UpdateAsync(Player player);
        Task DeleteAsync(Guid id);
        Task<Player> GetAsync(Guid id);
        Task<IEnumerable<Player>> GetAllAsync();
        Task<IEnumerable<Player>> GetAllPlayersByHeroId(Guid heroId);
    }
}
