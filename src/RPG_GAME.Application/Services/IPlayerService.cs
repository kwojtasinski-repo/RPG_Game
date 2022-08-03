using RPG_GAME.Application.DTO.Players;

namespace RPG_GAME.Application.Services
{
    public interface IPlayerService
    {
        Task AddAsync(AddPlayerDto playerDto);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(PlayerDto playerDto);
        Task<PlayerDto> GetAsync(Guid id);
        Task<IEnumerable<PlayerDto>> GetAllAsync();
    }
}
