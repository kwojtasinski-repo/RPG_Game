using RPG_GAME.Application.DTO;
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

        public async Task AddAsync(HeroDetailsDto heroDto)
        {
            await _heroRepository.AddAsync(heroDto.AsEntity());
        }

        public async Task<HeroDetailsDto> GetAsync(Guid id)
        {
            var hero = await _heroRepository.GetAsync(id);
            return hero.AsDetailsDto();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _heroRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(HeroDetailsDto heroDto)
        {
            var hero = heroDto.AsEntity();
            await _heroRepository.UpdateAsync(hero);
        }
    }
}
