using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Common;
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
            heroDto.Id = Guid.NewGuid();
            Validate(heroDto);
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
            var heroExists = await _heroRepository.GetAsync(id);

            if (heroExists is null)
            {
                throw new HeroNotFoundException(id);
            }

            if (heroExists.PlayersAssignedTo.Any())
            {
                throw new HeroCannotBeDeletedException(id);
            }

            await _heroRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(HeroDto heroDto)
        {
            Validate(heroDto);
            var heroExists = _heroRepository.GetAsync(heroDto.Id);

            if (heroExists is null)
            {
                throw new HeroNotFoundException(heroDto.Id);
            }

            var hero = heroDto.AsEntity();
            await _heroRepository.UpdateAsync(hero);
        }

        private static void Validate(HeroDto heroDto)
        {
            ValidateState(heroDto.Attack);
            ValidateState(heroDto.HealLvl);
            ValidateState(heroDto.Health);
            ValidateState(heroDto.BaseRequiredExperience);

            ValidateStrategyIncresing(heroDto.Attack.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(heroDto.HealLvl.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(heroDto.Health.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(heroDto.BaseRequiredExperience.IncreasingState.StrategyIncreasing);

            foreach (var skill in heroDto.Skills)
            {
                ValidateStrategyIncresing(skill.IncreasingState.StrategyIncreasing);
            }
        }

        private static void ValidateState<T>(StateDto<T> state)
            where T : struct
        {
            if (state is null)
            {
                throw new InvalidHeroStateException();
            }

            if (state.IncreasingState is null)
            {
                throw new InvalidHeroIncreasingStateException();
            }
        }

        private static void ValidateStrategyIncresing(string strategyIncreasing)
        {
            var strategiesIncresing = Enum.GetNames<StrategyIncreasing>();

            if (!strategiesIncresing.Any(s => s == strategyIncreasing))
            {
                throw new InvalidHeroStrategyIncreasingException(strategyIncreasing);
            }
        }
    }
}
