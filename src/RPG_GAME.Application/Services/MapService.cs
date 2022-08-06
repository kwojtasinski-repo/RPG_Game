using RPG_GAME.Application.DTO.Maps;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Application.Exceptions.Maps;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Application.Events.Maps;

namespace RPG_GAME.Application.Services
{
    internal sealed class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IMessageBroker _messageBroker;

        public MapService(IMapRepository mapRepository, IEnemyRepository enemyRepository, IMessageBroker messageBroker)
        {
            _mapRepository = mapRepository;
            _enemyRepository = enemyRepository;
            _messageBroker = messageBroker;
        }

        public async Task<MapDto> GetAsync(Guid id)
        {
            return (await _mapRepository.GetAsync(id))?.AsDto();
        }

        public async Task<IEnumerable<MapDto>> GetAllAsync()
        {
            return (await _mapRepository.GetAllAsync())
                .Select(m => m.AsDto());
        }

        public async Task AddAsync(AddMapDto mapDto)
        {
            var enemies = new List<Enemies>();

            foreach (var enemyDto in mapDto.Enemies)
            {
                var enemy = await _enemyRepository.GetAsync(enemyDto.EnemyId);

                if (enemy is null)
                {
                    throw new EnemyNotFoundException(enemyDto.EnemyId);
                }

                enemies.Add(new Enemies(enemy.AsAssign(), enemyDto.Quantity));
            }

            var map = Map.Create(mapDto.Name, mapDto.Difficulty, enemies);
            await _mapRepository.AddAsync(map);
            mapDto.Id = map.Id;

            var tasks = new List<Task>();
            
            foreach (var enemy in enemies)
            {
                tasks.Add(_messageBroker.PublishAsync(new EnemyAddedToMap(enemy.Enemy.Id, map.Id, map.Name)));
            }

            await Task.WhenAll(tasks);
        }

        public async Task UpdateAsync(AddMapDto mapDto)
        {
            var map = await _mapRepository.GetAsync(mapDto.Id);
            
            if (map is null)
            {
                throw new MapNotFoundException(mapDto.Id);
            }

            map.ChangeName(mapDto.Name);
            map.ChangeDifficulty(mapDto.Difficulty);

            if (mapDto.Enemies is null)
            {
                await _mapRepository.UpdateAsync(map);
                return;
            }

            var enemiesAdded = new List<Guid>();
            foreach (var enemyDto in mapDto.Enemies)
            {
                var enemyExists = map.Enemies.Any(e => e.Enemy.Id == enemyDto.EnemyId);

                if (enemyExists)
                {
                    continue;
                }

                var enemy = await _enemyRepository.GetAsync(enemyDto.EnemyId);

                if (enemy is null)
                {
                    throw new EnemyNotFoundException(enemyDto.EnemyId);
                }

                map.AddEnemies(new Enemies(enemy.AsAssign(), enemyDto.Quantity));
                enemiesAdded.Add(enemy.Id);
            }

            var enemiesDeleted = new List<Guid>();
            var enemies = new List<Enemies>(map.Enemies);
            foreach(var enemy in enemies)
            {
                var enemyExists = mapDto.Enemies.Any(e => e.EnemyId == enemy.Enemy.Id);

                if (enemyExists)
                {
                    continue;
                }

                map.RemoveEnemy(enemy);
                enemiesDeleted.Add(enemy.Enemy.Id);
            }

            await _mapRepository.UpdateAsync(map);

            var tasks = new List<Task>();

            foreach (var enemyId in enemiesAdded)
            {
                tasks.Add(_messageBroker.PublishAsync(new EnemyAddedToMap(enemyId, map.Id, map.Name)));
            }
            
            foreach (var enemyId in enemiesDeleted)
            {
                tasks.Add(_messageBroker.PublishAsync(new EnemyRemovedFromMap(enemyId, map.Id, map.Name)));
            }

            await Task.WhenAll(tasks);
        }
    }
}
