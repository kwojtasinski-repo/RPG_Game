using RPG_GAME.Application.DTO.Heroes;

namespace RPG_GAME.Application.Services
{
    public interface IHeroService
    {
        Task AddAsync(HeroDto heroDto);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(HeroDto heroDto);
        Task<HeroDetailsDto> GetAsync(Guid id);
        Task<IEnumerable<HeroDto>> GetAllAsync();
    }
}
