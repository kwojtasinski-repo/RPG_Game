using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class HeroService : IHeroService
    {
        private readonly IHeroRepository _heroRepository;

        public HeroService(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }

        public async Task AddAsync(HeroDto heroDto)
        {
            await _heroRepository.AddAsync(heroDto.AsEntity());
        }

        public async Task<HeroDetailsDto> GetAsync(Guid id)
        {
            var hero = await _heroRepository.GetAsync(id);
            return hero.AsDetailsDto();
        }

        public async Task<IEnumerable<HeroDto>> GetAllAsync()
        {
            var heroes = await _heroRepository.GetAllAsync();
            return heroes.Select(h => h.AsDto());
        }

        public async Task RemoveAsync(Guid id)
        {
            await _heroRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(HeroDto heroDto)
        {
            var hero = heroDto.AsEntity();
            await _heroRepository.UpdateAsync(hero);
        }
    }
}
