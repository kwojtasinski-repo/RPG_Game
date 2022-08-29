using MongoDB.Driver.Linq;
using MongoDB.Driver;
using RPG_GAME.Infrastructure.Mongo.Documents.Enemies;
using RPG_GAME.Infrastructure.Mongo.Mappings;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal class EnemyRepository : IEnemyRepository
    {
        private readonly IMongoRepository<EnemyDocument, Guid> _repository;

        public EnemyRepository(IMongoRepository<EnemyDocument, Guid> repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Enemy enemy)
        {
            var document = enemy.AsDocument();
            await _repository.AddAsync(document);
        }

        public async Task<Enemy> GetAsync(Guid id)
        {
            var enemy = await _repository.GetAsync(id);
            return enemy?.AsEntity();
        }

        public async Task<IEnumerable<Enemy>> GetAllAsync()
        {
            var enemies = await _repository.Collection.AsQueryable().ToListAsync();
            return enemies.Select(e => e.AsEntity());
        }

        public async Task UpdateAsync(Enemy enemy)
        {
            var document = enemy.AsDocument();
            await _repository.UpdateAsync(document);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Enemy>> GetByMapIdAsync(Guid mapId)
        {
            var enemies = await _repository.Collection.AsQueryable()
                .Where(e => e.MapsAssignedTo.Contains(mapId)).ToListAsync();
            return enemies.Select(e => e.AsEntity());
        }
    }
}
