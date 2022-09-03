using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class BattleEventRepositoryStub : IBattleEventRepository
    {
        private readonly IList<BattleEvent> _battleEvents = new List<BattleEvent>();

        public async Task AddAsync(BattleEvent battleEvent)
        {
            await Task.CompletedTask;
            _battleEvents.Add(battleEvent);
        }

        public async Task<BattleEvent> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _battleEvents.SingleOrDefault(b => b.BattleId == id);
        }

        public async Task<IEnumerable<BattleEvent>> GetByBattleIdAsync(Guid battleId)
        {
            await Task.CompletedTask;
            return _battleEvents.Where(b => b.BattleId == battleId).ToList();
        }
    }
}
