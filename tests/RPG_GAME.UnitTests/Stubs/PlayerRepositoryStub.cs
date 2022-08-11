using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class PlayerRepositoryStub : IPlayerRepository
    {
        private readonly IList<Player> _players = new List<Player>();

        public Task AddAsync(Player player)
        {
            _players.Add(player);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _players.Remove(_players.Single(p => p.Id == id));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Player>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Player>>(_players);
        }

        public Task<Player> GetAsync(Guid id)
        {
            return Task.FromResult(_players.FirstOrDefault(p => p.Id == id));
        }

        public Task UpdateAsync(Player player)
        {
            return Task.CompletedTask;
        }
    }
}
