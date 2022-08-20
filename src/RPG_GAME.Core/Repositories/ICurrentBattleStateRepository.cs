using RPG_GAME.Core.Entities.Battles;
using System;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface ICurrentBattleStateRepository
    {
        Task AddAsync(CurrentBattleState currentBattleState);
        Task UpdateAsync(CurrentBattleState currentBattleState);
        Task<CurrentBattleState> GetAsync(Guid id);
    }
}
