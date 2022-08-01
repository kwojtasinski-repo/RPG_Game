using RPG_GAME.Application.DTO;

namespace RPG_GAME.Application.Services
{
    public interface IEnemyService
    {
        Task AddAsync(EnemyDetailsDto enemyDto);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(EnemyDetailsDto enemyDto);
        Task<EnemyDetailsDto> GetAsync(Guid id);
        Task<IEnumerable<EnemyDto>> GetAllAsync();
    }
}
