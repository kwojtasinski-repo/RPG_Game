using RPG_GAME.Application.DTO.Enemies;

namespace RPG_GAME.Application.Services
{
    public interface IEnemyService
    {
        Task AddAsync(EnemyDto enemyDto);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(EnemyDto enemyDto);
        Task<EnemyDetailsDto> GetAsync(Guid id);
        Task<IEnumerable<EnemyDto>> GetAllAsync();
    }
}
