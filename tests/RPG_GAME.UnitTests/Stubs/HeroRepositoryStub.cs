using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class HeroRepositoryStub : IHeroRepository
    {
        private readonly IList<Hero> _heroes = new List<Hero>();

        public Task AddAsync(Hero hero)
        {
            _heroes.Add(hero);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _heroes.Remove(_heroes.Single(h => h.Id == id));
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Hero>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Hero>>(_heroes);
        }

        public Task<Hero> GetAsync(Guid id)
        {
            return Task.FromResult(_heroes.SingleOrDefault(h => h.Id == id));
        }

        public Task UpdateAsync(Hero hero)
        {
            return Task.CompletedTask;
        }
    }
}
