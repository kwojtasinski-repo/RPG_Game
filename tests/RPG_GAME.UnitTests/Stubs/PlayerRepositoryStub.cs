﻿using RPG_GAME.Core.Entities.Players;
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

        public Task<bool> ExistsAsync(Guid userId)
        {
            return Task.FromResult(_players.Any(p => p.UserId == userId));
        }

        public Task<IEnumerable<Player>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Player>>(_players);
        }

        public Task<IEnumerable<Player>> GetAllPlayersByHeroId(Guid heroId)
        {
            return Task.FromResult(_players.Where(p => p.Hero.Id == heroId));
        }

        public Task<Player> GetAsync(Guid id)
        {
            return Task.FromResult(_players.FirstOrDefault(p => p.Id == id));
        }

        public async Task<Player> GetByUserId(Guid userId)
        {
            await Task.CompletedTask;
            return _players.SingleOrDefault(p => p.UserId == userId);
        }

        public Task UpdateAsync(Player player)
        {
            return Task.CompletedTask;
        }
    }
}
