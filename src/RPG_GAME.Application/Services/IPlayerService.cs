using RPG_GAME.Application.DTO.Players;

namespace RPG_GAME.Application.Services
{
    public interface IPlayerService
    {
        Task AddAsync(AddPlayerDto playerDto);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(UpdatePlayerDto playerDto);
        Task<PlayerDto> GetAsync(Guid id);
        Task<PlayerDto> GetByUserAsync(Guid userId);
        Task<IEnumerable<PlayerDto>> GetAllAsync();
    }
}
