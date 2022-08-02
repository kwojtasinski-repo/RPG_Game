using RPG_GAME.Application.DTO.Maps;

namespace RPG_GAME.Application.Services
{
    public interface IMapService
    {
        Task<MapDto> GetAsync(Guid id);
        Task<IEnumerable<MapDto>> GetAllAsync(Guid id);
    }
}
