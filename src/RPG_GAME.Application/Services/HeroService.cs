using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Events.Heroes;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class HeroService : IHeroService
    {
        private readonly IHeroRepository _heroRepository;
        private readonly IMessageBroker _messageBroker;

        public HeroService(IHeroRepository heroRepository, IMessageBroker messageBroker)
        {
            _heroRepository = heroRepository;
            _messageBroker = messageBroker;
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
            return hero?.AsDetailsDto();
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
            var heroExists = await _heroRepository.GetAsync(heroDto.Id);

            if (heroExists is null)
            {
                throw new HeroNotFoundException(heroDto.Id);
            }

            heroExists.ChangeHeroName(heroDto.HeroName);
            heroExists.ChangeAttack(heroDto.Attack.AsEntity());
            heroExists.ChangeHealth(heroDto.Health.AsEntity());
            heroExists.ChangeHealLvl(heroDto.HealLvl.AsEntity());
            heroExists.ChangeBaseRequiredExperience(heroDto.BaseRequiredExperience.AsEntity());

            if (heroDto.Skills is null)
            {
                await _heroRepository.UpdateAsync(heroExists);
                await _messageBroker.PublishAsync(new HeroUpdated(heroExists.Id, heroExists.HeroName));
                return;
            }

            foreach (var skill in heroDto.Skills)
            {
                var skillHeroExists = heroExists.Skills.Any(s => s.Id == skill.Id);

                if (skillHeroExists)
                {
                    continue;
                }
                
                heroExists.AddSkill(skill.AsEntity());
            }

            var heroSkills = new List<SkillHero>(heroExists.Skills);
            foreach (var skill in heroSkills)
            {
                var skillHeroExists = heroDto.Skills.Any(s => s.Id == skill.Id);

                if (skillHeroExists)
                {
                    continue;
                }

                heroExists.RemoveSkill(skill);
            }

            await _heroRepository.UpdateAsync(heroExists);
            await _messageBroker.PublishAsync(new HeroUpdated(heroExists.Id, heroExists.HeroName, heroExists.Skills));
        }

        private static void Validate(HeroDto heroDto)
        {
            ValidateState(heroDto.Attack, nameof(HeroDto.Attack));
            ValidateState(heroDto.HealLvl, nameof(HeroDto.HealLvl));
            ValidateState(heroDto.Health, nameof(HeroDto.Health));
            ValidateState(heroDto.BaseRequiredExperience, nameof(HeroDto.BaseRequiredExperience));

            ValidateStrategyIncresing(heroDto.Attack.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(heroDto.HealLvl.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(heroDto.Health.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(heroDto.BaseRequiredExperience.IncreasingState.StrategyIncreasing);

            if (heroDto.Skills is null)
            {
                return;    
            }

            foreach (var skill in heroDto.Skills)
            {
                ValidateStrategyIncresing(skill.IncreasingState.StrategyIncreasing);
            }
        }

        private static void ValidateState<T>(StateDto<T> state, string name)
            where T : struct
        {
            if (state is null)
            {
                throw new InvalidHeroStateException(name);
            }

            if (state.IncreasingState is null)
            {
                throw new InvalidHeroIncreasingStateException(name);
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
