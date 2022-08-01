using RPG_GAME.Application.DTO;

namespace RPG_GAME.Application.Services
{
    public interface IHeroService
    {
        Task AddAsync(HeroDetailsDto heroDto);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(HeroDetailsDto heroDto);
        Task<HeroDetailsDto> GetAsync(Guid id);
    }
}
