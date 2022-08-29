using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Infrastructure.Mongo.Documents.Battles;
using RPG_GAME.Infrastructure.Mongo.Mappings;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal class CurrentBattleStateRepository : ICurrentBattleStateRepository
    {
        private readonly IMongoRepository<CurrentBattleStateDocument, Guid> _repository;

        public CurrentBattleStateRepository(IMongoRepository<CurrentBattleStateDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(CurrentBattleState currentBattleState)
        {
            var document = currentBattleState.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<CurrentBattleState> GetByBattleIdAsync(Guid battleId)
        {
            var currentBattleState = await _repository.Collection.AsQueryable()
                .SingleOrDefaultAsync(cbs => cbs.BattleId == battleId);
            return currentBattleState?.AsEntity();
        }

        public async Task UpdateAsync(CurrentBattleState currentBattleState)
        {
            var document = currentBattleState.AsDocument();
            await _repository.UpdateAsync(document);
        }
    }
}
