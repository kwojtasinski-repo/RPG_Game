using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class EnemyService : IEnemyService
    {
        private readonly IEnemyRepository _enemyRepository;

        public EnemyService(IEnemyRepository enemyRepository)
        {
            _enemyRepository = enemyRepository;
        }

        public async Task AddAsync(EnemyDto enemyDto)
        {
            await _enemyRepository.AddAsync(enemyDto.AsEntity());
        }

        public async Task<EnemyDetailsDto> GetAsync(Guid id)
        {
            var enemy = await _enemyRepository.GetAsync(id);
            return enemy.AsDetailsDto();
        }

        public async Task<IEnumerable<EnemyDto>> GetAllAsync()
        {
            var enemies = await _enemyRepository.GetAllAsync();
            return enemies.Select(e => e.AsDto());
        }

        public async Task RemoveAsync(Guid id)
        {
            await _enemyRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(EnemyDto enemyDto)
        {
            await _enemyRepository.UpdateAsync(enemyDto.AsEntity());
        }
    }
}
