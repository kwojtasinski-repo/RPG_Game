using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Infrastructure.Mongo.Documents.Battles;
using RPG_GAME.Infrastructure.Mongo.Mappings;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal class BattleRepository : IBattleRepository
    {
        private readonly IMongoRepository<BattleDocument, Guid> _repository;

        public BattleRepository(IMongoRepository<BattleDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Battle battle)
        {
            var document = battle.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<Battle> GetAsync(Guid id)
        {
            var battle = await _repository.GetAsync(id);
            return battle?.AsEntity();
        }

        public async Task<IEnumerable<Battle>> GetByUserIdAsync(Guid userId)
        {
            var battles = await _repository.Collection.AsQueryable().Where(b => b.UserId == userId).ToListAsync();
            return battles.Select(b => b.AsEntity());
        }

        public async Task UpdateAsync(Battle battle)
        {
            var document = battle.AsDocument();
            await _repository.UpdateAsync(document);
        }
    }
}
