using RPG_GAME.Core.Entities.Battles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IBattleEventRepository
    {
        Task AddAsync(BattleEvent battleEvent);
        Task<BattleEvent> GetAsync(Guid id);
        Task<IEnumerable<BattleEvent>> GetByBattleIdAsync(Guid battleId);
    }
}
