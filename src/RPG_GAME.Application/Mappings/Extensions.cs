using RPG_GAME.Application.DTO;
using RPG_GAME.Core.Entities;

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
                Hero = playerDto.Hero.AsEntiy(),
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

        public static Hero AsEntiy(this HeroDto heroDto)
        {
            return new Hero()
            {
                Id = heroDto.Id,
                Attack = new Field<int>() { Value = heroDto.Attack },
                HeroName = heroDto.HeroName,
                HealLvl = new Field<int>() { Value = heroDto.HealLvl },
                Health = new Field<int>() { Value = heroDto.Health }
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

        public static Field<T> AsField<T>(this FieldDto<T> fieldDto)
            where T : struct
        {
            return new Field<T>
            {
                IncreasingStats = fieldDto.IncreasingStats.AsIncreasingStats(),
                Value = fieldDto.Value
            };
        }

        public static IncreasingStats<T> AsIncreasingStats<T>(this IncreasingStatsDto<T> fieldDto)
            where T : struct
        {
            return new IncreasingStats<T>
            {
                Value = fieldDto.Value,
                StrategyIncreasing = fieldDto.StrategyIncreasing
            };
        }

        public static Skill AsSkill(this SkillDetailsDto skill)
        {
            return new Skill()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                Probability = skill.Probability,
                IncreasingStats = skill.IncreasingStats.AsIncreasingStats()
            };
        }

        public static HeroDto AsDto(this Hero hero)
        {
            return new HeroDto()
            {
                Id = hero.Id,
                Attack = hero.Attack.Value,
                HealLvl = hero.HealLvl.Value,
                Health = hero.Health.Value,
                HeroName = hero.HeroName
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
                Skills = hero.Skills.Select(s => s.AsDetailsDto())
            };
        }

        public static FieldDto<T> AsDto<T>(this Field<T> field)
            where T : struct
        {
            return new FieldDto<T>
            {
                IncreasingStats = field.IncreasingStats.AsDto(),
                Value = field.Value
            };
        }

        public static IncreasingStatsDto<T> AsDto<T>(this IncreasingStats<T> field)
            where T : struct
        {
            return new IncreasingStatsDto<T>
            {
                Value = field.Value,
                StrategyIncreasing = field.StrategyIncreasing
            };
        }

        public static SkillDetailsDto AsDetailsDto(this Skill skill)
        {
            return new SkillDetailsDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                Probability = skill.Probability,
                IncreasingStats = skill.IncreasingStats.AsDto()
            };
        }

        public static Enemy AsEntity(this EnemyDetailsDto enemyDto)
        {
            return new Enemy()
            {
                Id = enemyDto.Id,
                EnemyName = enemyDto.EnemyName,
                BaseAttack = enemyDto.BaseAttack.AsField(),
                BaseHealLvl = enemyDto.BaseHealLvl.AsField(),
                BaseHealth = enemyDto.BaseHealth.AsField(),
                Difficulty = enemyDto.Difficulty,
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
                Difficulty = enemy.Difficulty,
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
                Difficulty = enemy.Difficulty,
                Experience = enemy.Experience.AsDto(),
                Skills = enemy.Skills.Select(s => s.AsDetailsDto())
            };
        }
    }
}
