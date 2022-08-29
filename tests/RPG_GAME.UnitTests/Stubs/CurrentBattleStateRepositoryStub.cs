using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class CurrentBattleStateRepositoryStub : ICurrentBattleStateRepository
    {
        private readonly IList<CurrentBattleState> _currentBattleStates = new List<CurrentBattleState>();

        public async Task AddAsync(CurrentBattleState currentBattleState)
        {
            await Task.CompletedTask;
            _currentBattleStates.Add(currentBattleState);
        }

        public async Task<CurrentBattleState> GetByBattleIdAsync(Guid battleId)
        {
            await Task.CompletedTask;
            return _currentBattleStates.SingleOrDefault(cbs => cbs.BattleId == battleId);
        }

        public async Task UpdateAsync(CurrentBattleState currentBattleState)
        {
            await Task.CompletedTask;
        }
    }
}
