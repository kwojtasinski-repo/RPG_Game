using RPG_GAME.Application.DTO;
using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Mappings
{
    internal static class Extensions
    {
        public static Player AsEntity(this PlayerDto playerDto)
        {
            return new Player()
            {
                Id = playerDto.Id,
                Name = playerDto.Name,
                UserId = playerDto.UserId,
                CurrentExp = playerDto.CurrentExp,
                Level = playerDto.Level,
                RequiredExp = playerDto.RequiredExp,
                Hero = playerDto.Hero.AsAssignEntity(),
            };
        }

        public static PlayerDto AsDto(this Player player)
        {
            return new PlayerDto()
            {
                Id = player.Id,
                UserId = player.UserId,
                Name = player.Name,
                Level = player.Level,
                CurrentExp = player.CurrentExp,
                RequiredExp = player.RequiredExp,
                Hero = player.Hero.AsDto()
            };
        }

        public static Hero AsEntity(this HeroDto heroDto)
        {
            return new Hero()
            {
                Id = heroDto.Id,
                Attack = heroDto.Attack.AsField(),
                HeroName = heroDto.HeroName,
                HealLvl = heroDto.HealLvl.AsField(),
                Health = heroDto.Health.AsField(),
                BaseRequiredExperience = heroDto.BaseRequiredExperience.AsField(),
                Skills = heroDto.Skills.Select(s => s.AsSkill())
            };
        }

        public static HeroAssign AsAssignEntity(this HeroDto heroDto)
        {
            return new HeroAssign()
            {//TODO
                Id = heroDto.Id,
                HeroName = heroDto.HeroName,
            };
        }

        public static Hero AsEntity(this HeroDetailsDto heroDto)
        {
            return new Hero()
            {
                Id = heroDto.Id,
                Attack = heroDto.Attack.AsField(),
                HeroName = heroDto.HeroName,
                HealLvl = heroDto.HealLvl.AsField(),
                Health = heroDto.Health.AsField(),
                BaseRequiredExperience = heroDto.BaseRequiredExperience.AsField(),
                Skills = heroDto.Skills.Select(s => s.AsSkill())
            };
        }

        public static State<T> AsField<T>(this StateDto<T> fieldDto)
            where T : struct
        {
            return new State<T>
            {
                IncreasingState = fieldDto.IncreasingStats.AsIncreasingStats(),
                Value = fieldDto.Value
            };
        }

        public static IncreasingState<T> AsIncreasingStats<T>(this IncreasingStatsDto<T> fieldDto)
            where T : struct
        {
            return new IncreasingState<T>
            {
                Value = fieldDto.Value,
                StrategyIncreasing = Enum.Parse<StrategyIncreasing>(fieldDto.StrategyIncreasing)
            };
        }

        public static SkillEnemy AsSkill(this SkillDetailsEnemyDto skill)
        {
            return new SkillEnemy()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                Probability = skill.Probability,
                IncreasingState = skill.IncreasingStats.AsIncreasingStats()
            };
        }

        public static SkillHero AsSkill(this SkillDetailsHeroDto skill)
        {
            return new SkillHero()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                IncreasingState = skill.IncreasingStats.AsIncreasingStats()
            };
        }

        public static HeroDto AsDto(this HeroAssign hero)
        {
            return new HeroDto()
            {//TODO
                Id = hero.Id,
                HeroName = hero.HeroName
            };
        }

        public static HeroDto AsDto(this Hero hero)
        {
            return new HeroDto()
            {
                Id = hero.Id,
                Attack = hero.Attack.AsDto(),
                HealLvl = hero.HealLvl.AsDto(),
                Health = hero.Health.AsDto(),
                HeroName = hero.HeroName,
                BaseRequiredExperience = hero.BaseRequiredExperience.AsDto(),
                Skills = hero.Skills.Select(s => s.AsDetailsDto())
            };
        }

        public static HeroDetailsDto AsDetailsDto(this Hero hero)
        {
            return new HeroDetailsDto()
            {
                Id = hero.Id,
                Attack = hero.Attack.AsDto(),
                HealLvl = hero.HealLvl.AsDto(),
                Health = hero.Health.AsDto(),
                HeroName = hero.HeroName,
                BaseRequiredExperience = hero.BaseRequiredExperience.AsDto(),
                Skills = hero.Skills.Select(s => s.AsDetailsDto()),
                PlayersAssignedTo = hero.PlayersAssignedTo
            };
        }

        public static StateDto<T> AsDto<T>(this State<T> field)
            where T : struct
        {
            return new StateDto<T>
            {
                IncreasingStats = field.IncreasingState.AsDto(),
                Value = field.Value
            };
        }

        public static IncreasingStatsDto<T> AsDto<T>(this IncreasingState<T> field)
            where T : struct
        {
            return new IncreasingStatsDto<T>
            {
                Value = field.Value,
                StrategyIncreasing = field.StrategyIncreasing.ToString()
            };
        }

        public static SkillDetailsEnemyDto AsDetailsDto(this SkillEnemy skill)
        {
            return new SkillDetailsEnemyDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                Probability = skill.Probability,
                IncreasingStats = skill.IncreasingState.AsDto()
            };
        }

        public static SkillDetailsHeroDto AsDetailsDto(this SkillHero skill)
        {
            return new SkillDetailsHeroDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                IncreasingStats = skill.IncreasingState.AsDto()
            };
        }

        public static Enemy AsEntity(this EnemyDto enemyDto)
        {
            return new Enemy()
            {
                Id = enemyDto.Id,
                EnemyName = enemyDto.EnemyName,
                BaseAttack = enemyDto.BaseAttack.AsField(),
                BaseHealLvl = enemyDto.BaseHealLvl.AsField(),
                BaseHealth = enemyDto.BaseHealth.AsField(),
                Difficulty = Enum.Parse<Difficulty>(enemyDto.Difficulty),
                Experience = enemyDto.Experience.AsField(),
                Skills = enemyDto.Skills.Select(s => s.AsSkill())
            };
        }

        public static EnemyDto AsDto(this Enemy enemy)
        {
            return new EnemyDto()
            {
                Id = enemy.Id,
                EnemyName = enemy.EnemyName,
                Difficulty = enemy.Difficulty.ToString(),
                BaseAttack = enemy.BaseAttack.AsDto(),
                BaseHealLvl = enemy.BaseHealLvl.AsDto(),
                BaseHealth = enemy.BaseHealth.AsDto(),
                Experience = enemy.Experience.AsDto(),
                Skills = enemy.Skills.Select(s => s.AsDetailsDto())
            };
        }

        public static EnemyDetailsDto AsDetailsDto(this Enemy enemy)
        {
            return new EnemyDetailsDto()
            {
                Id = enemy.Id,
                EnemyName = enemy.EnemyName,
                BaseAttack = enemy.BaseAttack.AsDto(),
                BaseHealLvl = enemy.BaseHealLvl.AsDto(),
                BaseHealth = enemy.BaseHealth.AsDto(),
                Difficulty = enemy.Difficulty.ToString(),
                Experience = enemy.Experience.AsDto(),
                Skills = enemy.Skills.Select(s => s.AsDetailsDto()),
                MapsAssignedTo = enemy.MapsAssignedTo
            };
        }
    }
}
