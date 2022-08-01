using MongoDB.Driver;
using RPG_Game.Infrastructure.Mongo.Documents;
using RPG_Game.Infrastructure.Mongo.Mappings;
using RPG_GAME.Core.Entities;
using RPG_GAME.Core.Repositories;

namespace RPG_Game.Infrastructure.Mongo.Repositories
{
    internal class HeroRepository : IHeroRepository
    {
        private readonly IMongoRepository<HeroDocument, Guid> _repository;

        public HeroRepository(IMongoRepository<HeroDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Hero hero)
        {
            var document = hero.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<Hero> GetAsync(Guid id)
        {
            var hero = await _repository.GetAsync(id);
            return hero.AsEntity();
        }

        public async Task<IEnumerable<Hero>> GetAllAsync()
        {
            var heroes = await _repository.Collection.AsQueryable().ToListAsync();
            return heroes.Select(e => e.AsEntity());
        }

        public async Task UpdateAsync(Hero hero)
        {
            var document = hero.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
