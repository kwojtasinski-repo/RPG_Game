using MongoDB.Driver;
using RPG_GAME.Infrastructure.Mongo.Mappings;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Infrastructure.Mongo.Documents.Players;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal class PlayerRepository : IPlayerRepository
    {
        private readonly IMongoRepository<PlayerDocument, Guid> _repository;

        public PlayerRepository(IMongoRepository<PlayerDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task Add(Player player)
        {
            var document = player.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<Player> Get(Guid id)
        {
            var player = await _repository.GetAsync(id);
            return player.AsEntity();
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            var players = await _repository.Collection.AsQueryable().ToListAsync();
            return players.Select(e => e.AsEntity());
        }

        public async Task Update(Player player)
        {
            var document = player.AsDocument();
            await _repository.UpdateAsync(document);
        }
    }
}
