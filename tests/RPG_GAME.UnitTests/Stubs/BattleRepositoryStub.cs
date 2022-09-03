using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class BattleRepositoryStub : IBattleRepository
    {
        private readonly IList<Battle> _battles = new List<Battle>();

        public async Task AddAsync(Battle battle)
        {
            await Task.CompletedTask;
            _battles.Add(battle);
        }

        public async Task<Battle> GetAsync(Guid id)
        {
            await Task.CompletedTask;
            return _battles.SingleOrDefault(b => b.Id == id);
        }

        public async Task<IEnumerable<Battle>> GetByUserIdAsync(Guid userId)
        {
            await Task.CompletedTask;
            return _battles.Where(e => e.UserId == userId).ToList();
        }

        public Task UpdateAsync(Battle battle)
        {
            return Task.CompletedTask;
        }
    }
}
