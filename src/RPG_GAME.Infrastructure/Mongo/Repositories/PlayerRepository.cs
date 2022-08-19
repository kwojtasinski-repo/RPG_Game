using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        public async Task AddAsync(Player player)
        {
            var document = player.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<Player> GetAsync(Guid id)
        {
            var player = await _repository.GetAsync(id);
            return player?.AsEntity();
        }

        public async Task<IEnumerable<Player>> GetAllAsync()
        {
            var players = await _repository.Collection.AsQueryable().ToListAsync();
            return players.Select(e => e.AsEntity());
        }

        public async Task UpdateAsync(Player player)
        {
            var document = player.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Player>> GetAllPlayersByHeroId(Guid heroId)
        {
            var players = await _repository.Collection.AsQueryable().Where(p => p.Hero.Id == heroId).ToListAsync();
            return players.Select(p => p.AsEntity());
        }

        public async Task<Player> GetByUserId(Guid userId)
        {
            return (await _repository.Collection
                .AsQueryable()
                .SingleOrDefaultAsync(u => u.UserId == userId)).AsEntity();
        }
    }
}
