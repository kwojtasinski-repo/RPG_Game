using RPG_GAME.Application.DTO;
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
                Hero = playerDto.Hero.AsAssignEntiy(),
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
                Attack = new State<int>() { Value = heroDto.Attack },
                HeroName = heroDto.HeroName,
                HealLvl = new State<int>() { Value = heroDto.HealLvl },
                Health = new State<int>() { Value = heroDto.Health }
            };
        }

        public static HeroAssign AsAssignEntiy(this HeroDto heroDto)
        {
            return new HeroAssign()
            {
                Id = heroDto.Id,
                Attack = heroDto.Attack,
                HeroName = heroDto.HeroName,
                HealLvl =  heroDto.HealLvl,
                Health = heroDto.Health
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

        public static State<T> AsField<T>(this FieldDto<T> fieldDto)
            where T : struct
        {
            return new State<T>
            {
                IncreasingStats = fieldDto.IncreasingStats.AsIncreasingStats(),
                Value = fieldDto.Value
            };
        }

        public static IncreasingState<T> AsIncreasingStats<T>(this IncreasingStatsDto<T> fieldDto)
            where T : struct
        {
            return new IncreasingState<T>
            {
                Value = fieldDto.Value,
                StrategyIncreasing = fieldDto.StrategyIncreasing
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
                IncreasingStats = skill.IncreasingStats.AsIncreasingStats()
            };
        }

        public static SkillHero AsSkill(this SkillDetailsHeroDto skill)
        {
            return new SkillHero()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
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

        public static HeroDto AsDto(this HeroAssign hero)
        {
            return new HeroDto()
            {
                Id = hero.Id,
                Attack = hero.Attack,
                HealLvl = hero.HealLvl,
                Health = hero.Health,
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

        public static FieldDto<T> AsDto<T>(this State<T> field)
            where T : struct
        {
            return new FieldDto<T>
            {
                IncreasingStats = field.IncreasingStats.AsDto(),
                Value = field.Value
            };
        }

        public static IncreasingStatsDto<T> AsDto<T>(this IncreasingState<T> field)
            where T : struct
        {
            return new IncreasingStatsDto<T>
            {
                Value = field.Value,
                StrategyIncreasing = field.StrategyIncreasing
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
                IncreasingStats = skill.IncreasingStats.AsDto()
            };
        }

        public static SkillDetailsHeroDto AsDetailsDto(this SkillHero skill)
        {
            return new SkillDetailsHeroDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
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
