using RPG_GAME.Application.DTO.Common;
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
                Hero = player.Hero.AsAssignDto()
            };
        }

        public static Hero AsEntity(this HeroDto heroDto)
        {
            return new Hero(heroDto.Id, heroDto.HeroName,
                heroDto.Attack.AsEntity(), heroDto.HealLvl.AsEntity(),
                heroDto.Health.AsEntity(), heroDto.BaseRequiredExperience.AsEntity(),
                heroDto.Skills.Select(s => s.AsEntity()));
        }

        public static HeroAssign AsAssign(this Hero hero)
        {
            return new HeroAssign()
            {
                Id = hero.Id,
                HeroName = hero.HeroName,
                Attack = hero.Attack.Value,
                HealLvl = hero.HealLvl.Value,
                Health = hero.Health.Value,
                BaseRequiredExperience = hero.BaseRequiredExperience.Value,
                Skills = hero.Skills.Select(s => s.AsAssign())
            };
        }

        public static HeroAssign AsAssignEntity(this HeroAssignDto heroDto)
        {
            return new HeroAssign()
            {
                Id = heroDto.Id,
                HeroName = heroDto.HeroName,
                Attack = heroDto.Attack,
                HealLvl = heroDto.HealLvl,
                Health = heroDto.Health,
                BaseRequiredExperience = heroDto.BaseRequiredExperience,
                Skills = heroDto.Skills.Select(s => s.AsAssignEntity())
            };
        }
        
        public static SkillHeroAssign AsAssignEntity(this SkillHeroAssignDto skillHeroDto)
        {
            return new SkillHeroAssign()
            {
                Id = skillHeroDto.Id,
                Name= skillHeroDto.Name,
                BaseAttack = skillHeroDto.BaseAttack
            };
        }

        public static SkillHeroAssign AsAssign(this SkillHero skillHero)
        {
            return new SkillHeroAssign()
            {
                Id = skillHero.Id,
                Name= skillHero.Name,
                BaseAttack = skillHero.BaseAttack
            };
        }

        public static HeroAssignDto AsAssignDto(this HeroAssign hero)
        {
            return new HeroAssignDto()
            {
                Id = hero.Id,
                HeroName = hero.HeroName,
                Attack = hero.Attack,
                HealLvl = hero.HealLvl,
                Health = hero.Health,
                BaseRequiredExperience = hero.BaseRequiredExperience,
                Skills = hero.Skills.Select(s => s.AsDto())
            };
        }

        public static SkillHeroAssignDto AsDto(this SkillHeroAssign skill)
        {
            return new SkillHeroAssignDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack
            };
        }

        public static State<T> AsEntity<T>(this StateDto<T> fieldDto)
            where T : struct
        {
            return new State<T>
            {
                IncreasingState = fieldDto.IncreasingState.AsEntity(),
                Value = fieldDto.Value
            };
        }

        public static IncreasingState<T> AsEntity<T>(this IncreasingStateDto<T> fieldDto)
            where T : struct
        {
            return new IncreasingState<T>
            {
                Value = fieldDto.Value,
                StrategyIncreasing = Enum.Parse<StrategyIncreasing>(fieldDto.StrategyIncreasing)
            };
        }

        public static SkillEnemy AsEntity(this SkillEnemyDto skill)
        {
            return new SkillEnemy()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                Probability = skill.Probability,
                IncreasingState = skill.IncreasingState.AsEntity()
            };
        }

        public static SkillHero AsEntity(this SkillHeroDto skill)
        {
            return new SkillHero()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                IncreasingState = skill.IncreasingState.AsEntity()
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
                Skills = hero.Skills.Select(s => s.AsDto())
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
                Skills = hero.Skills.Select(s => s.AsDto()),
                PlayersAssignedTo = hero.PlayersAssignedTo
            };
        }

        public static StateDto<T> AsDto<T>(this State<T> field)
            where T : struct
        {
            return new StateDto<T>
            {
                IncreasingState = field.IncreasingState.AsDto(),
                Value = field.Value
            };
        }

        public static IncreasingStateDto<T> AsDto<T>(this IncreasingState<T> field)
            where T : struct
        {
            return new IncreasingStateDto<T>
            {
                Value = field.Value,
                StrategyIncreasing = field.StrategyIncreasing.ToString()
            };
        }

        public static SkillEnemyDto AsDto(this SkillEnemy skill)
        {
            return new SkillEnemyDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                Probability = skill.Probability,
                IncreasingState = skill.IncreasingState.AsDto()
            };
        }

        public static SkillHeroDto AsDto(this SkillHero skill)
        {
            return new SkillHeroDto()
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
                IncreasingState = skill.IncreasingState.AsDto()
            };
        }

        public static Enemy AsEntity(this EnemyDto enemyDto)
        {
            return new Enemy()
            {
                Id = enemyDto.Id,
                EnemyName = enemyDto.EnemyName,
                BaseAttack = enemyDto.BaseAttack.AsEntity(),
                BaseHealLvl = enemyDto.BaseHealLvl.AsEntity(),
                BaseHealth = enemyDto.BaseHealth.AsEntity(),
                Difficulty = Enum.Parse<Difficulty>(enemyDto.Difficulty),
                Experience = enemyDto.Experience.AsEntity(),
                Skills = enemyDto.Skills.Select(s => s.AsEntity())
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
                Skills = enemy.Skills.Select(s => s.AsDto())
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
                Skills = enemy.Skills.Select(s => s.AsDto()),
                MapsAssignedTo = enemy.MapsAssignedTo
            };
        }
    }
}
