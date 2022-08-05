using RPG_GAME.Application.DTO.Maps;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Application.Exceptions.Maps;

namespace RPG_GAME.Application.Services
{
    internal sealed class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;

        public MapService(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public async Task<MapDto> GetAsync(Guid id)
        {
            return (await _mapRepository.GetAsync(id)).AsDto();
        }

        public async Task<IEnumerable<MapDto>> GetAllAsync()
        {
            return (await _mapRepository.GetAllAsync())
                .Select(m => m.AsDto());
        }

        public async Task AddAsync(MapDto mapDto)
        {
            var map = Map.Create(mapDto.Name, mapDto.Difficulty, mapDto.Enemies?.Select(e => e.AsEntity()));
            await _mapRepository.AddAsync(map);
        }

        public async Task UpdateAsync(MapDto mapDto)
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

            foreach (var enemy in mapDto.Enemies)
            {
                var enemyExists = map.Enemies.Any(e => e.Enemy.Id == enemy.Enemy.Id);

                if (enemyExists)
                {
                    continue;
                }

                map.AddEnemies(enemy.AsEntity());
            }

            foreach(var enemy in map.Enemies)
            {
                var enemyExists = mapDto.Enemies.Any(e => e.Enemy.Id == enemy.Enemy.Id);

                if (enemyExists)
                {
                    continue;
                }

                map.RemoveEnemy(enemy);
            }

            await _mapRepository.UpdateAsync(map);
        }
    }
}
