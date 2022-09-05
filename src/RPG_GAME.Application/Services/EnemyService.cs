using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.Events.Enemies;
using RPG_GAME.Application.Exceptions.Enemies;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class EnemyService : IEnemyService
    {
        private readonly IEnemyRepository _enemyRepository;
        private readonly IMessageBroker _messageBroker;

        public EnemyService(IEnemyRepository enemyRepository, IMessageBroker messageBroker)
        {
            _enemyRepository = enemyRepository;
            _messageBroker = messageBroker;
        }

        public async Task AddAsync(EnemyDto enemyDto)
        {
            enemyDto.Id = Guid.NewGuid();
            Validate(enemyDto);
            await _enemyRepository.AddAsync(enemyDto.AsEntity());
        }

        public async Task<EnemyDetailsDto> GetAsync(Guid id)
        {
            var enemy = await _enemyRepository.GetAsync(id);
            return enemy?.AsDetailsDto();
        }

        public async Task<IEnumerable<EnemyDto>> GetAllAsync()
        {
            var enemies = await _enemyRepository.GetAllAsync();
            return enemies.Select(e => e.AsDto());
        }

        public async Task RemoveAsync(Guid id)
        {
            var enemyExists = await _enemyRepository.GetAsync(id);

            if (enemyExists is null)
            {
                throw new EnemyNotFoundException(id);
            }

            if (enemyExists.MapsAssignedTo.Any())
            {
                throw new EnemyCannotBeDeletedException(id);
            }

            await _enemyRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(EnemyDto enemyDto)
        {
            Validate(enemyDto);
            var enemyExists = await _enemyRepository.GetAsync(enemyDto.Id);

            if (enemyExists is null)
            {
                throw new EnemyNotFoundException(enemyDto.Id);
            }

            enemyExists.ChangeEnemyName(enemyDto.EnemyName);
            enemyExists.ChangeAttack(enemyDto.BaseAttack.AsEntity());
            enemyExists.ChangeDifficulty(enemyDto.Difficulty);
            enemyExists.ChangeCategory(enemyDto.Category);
            enemyExists.ChangeHealth(enemyDto.BaseHealth.AsEntity());
            enemyExists.ChangeHealLvl(enemyDto.BaseHealLvl.AsEntity());
            enemyExists.ChangeExperience(enemyDto.Experience.AsEntity());

            if (enemyDto.Skills is null)
            {
                await _enemyRepository.UpdateAsync(enemyExists);
                await _messageBroker.PublishAsync(new EnemyUpdated(enemyExists.Id, enemyExists.EnemyName, enemyExists.BaseAttack.Value, enemyExists.BaseHealth.Value, enemyExists.BaseHealLvl.Value,
                                                        enemyExists.Experience.Value, enemyExists.Difficulty, enemyExists.Category));
                return;
            }

            foreach (var enemySkill in enemyDto.Skills)
            {
                var exists = enemyExists.Skills.Any(s => s.Id == enemySkill.Id);

                if (exists)
                {
                    continue;
                }

                enemyExists.AddSkill(enemySkill.AsEntity());
            }

            var enemySkills = new List<SkillEnemy>(enemyExists.Skills);
            foreach (var enemySkill in enemySkills)
            {
                var exists = enemyDto.Skills.Any(s => s.Id == enemySkill.Id);

                if (exists)
                {
                    continue;
                }

                enemyExists.RemoveSkill(enemySkill);
            }

            await _enemyRepository.UpdateAsync(enemyExists);
            await _messageBroker.PublishAsync(new EnemyUpdated(enemyExists.Id, enemyExists.EnemyName, enemyExists.BaseAttack.Value, enemyExists.BaseHealth.Value, enemyExists.BaseHealLvl.Value,
                                                        enemyExists.Experience.Value, enemyExists.Difficulty, enemyExists.Category, enemyExists.Skills.Select(e => e.AsAssign())));
        }

        private static void Validate(EnemyDto enemyDto)
        {
            var difficultyLevels = Enum.GetNames<Difficulty>();
            
            if (!difficultyLevels.Any(d => d == enemyDto.Difficulty))
            {
                throw new InvalidEnemyDifficultyException(enemyDto.Difficulty);
            }

            ValidateState(enemyDto.BaseAttack, nameof(EnemyDto.BaseAttack));
            ValidateState(enemyDto.BaseHealLvl, nameof(EnemyDto.BaseHealLvl));
            ValidateState(enemyDto.BaseHealth, nameof(EnemyDto.BaseHealth));
            ValidateState(enemyDto.Experience, nameof(EnemyDto.Experience));

            ValidateStrategyIncresing(enemyDto.BaseAttack.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(enemyDto.BaseHealLvl.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(enemyDto.BaseHealth.IncreasingState.StrategyIncreasing);
            ValidateStrategyIncresing(enemyDto.Experience.IncreasingState.StrategyIncreasing);

            if (enemyDto.Skills is null)
            {
                return;
            }

            foreach (var skill in enemyDto.Skills)
            {
                ValidateStrategyIncresing(skill.IncreasingState.StrategyIncreasing);
            }
        }

        private static void ValidateStrategyIncresing(string strategyIncreasing)
        {
            var strategiesIncresing = Enum.GetNames<StrategyIncreasing>();

            if (!strategiesIncresing.Any(s => s == strategyIncreasing))
            {
                throw new InvalidEnemyStrategyIncreasingException(strategyIncreasing);
            }
        }

        private static void ValidateState<T>(StateDto<T> state, string name)
            where T : struct
        {
            if (state is null)
            {
                throw new InvalidEnemyStateException(name);
            }

            if (state.IncreasingState is null)
            {
                throw new InvalidEnemyIncreasingStateException(name);
            }
        }
    }
}
