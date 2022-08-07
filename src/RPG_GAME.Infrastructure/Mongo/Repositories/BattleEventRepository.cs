using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Infrastructure.Mongo.Documents.Battles;
using RPG_GAME.Infrastructure.Mongo.Mappings;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal class BattleEventRepository : IBattleEventRepository
    {
        private readonly IMongoRepository<BattleEventDocument, Guid> _repository;

        public async Task AddAsync(BattleEvent battleEvent)
        {
            var document = battleEvent.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<BattleEvent> GetAsync(Guid id)
        {
            var battleEvent = await _repository.GetAsync(id);
            return battleEvent?.AsEntity();
        }

        public async Task<IEnumerable<BattleEvent>> GetByBattleIdAsync(Guid battleId)
        {
            var battleEvents = await _repository.Collection.AsQueryable().Where(b => b.BattleId == battleId).ToListAsync();
            return battleEvents.Select(b => b.AsEntity());
        }
    }
}
