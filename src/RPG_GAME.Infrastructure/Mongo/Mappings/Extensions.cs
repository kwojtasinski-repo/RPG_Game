using RPG_GAME.Infrastructure.Mongo.Documents;
using RPG_GAME.Infrastructure.Mongo.Documents.Enemies;
using RPG_GAME.Infrastructure.Mongo.Documents.Heroes;
using RPG_GAME.Infrastructure.Mongo.Documents.Maps;
using RPG_GAME.Infrastructure.Mongo.Documents.Players;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;

namespace RPG_GAME.Infrastructure.Mongo.Mappings
{
    internal static class Extensions
    {
        public static UserDocument AsDocument(this User user)
        {
            return new UserDocument
            {
                Id = user.Id,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive
            };
        }

        public static User AsEntity(this UserDocument userDocument)
        {
            return new User
            {
                Id = userDocument.Id,
                Email = userDocument.Email,
                Password = userDocument.Password,
                Role = userDocument.Role,
                CreatedAt = userDocument.CreatedAt,
                IsActive = userDocument.IsActive
            };
        }
        
        public static PlayerDocument AsDocument(this Player player)
        {
            return new PlayerDocument
            {
                Id = player.Id,
                Name = player.Name,
                CurrentExp = player.CurrentExp,
                Hero = player.Hero.AsDocument(),
                Level = player.Level,
                RequiredExp = player.RequiredExp,
                UserId = player.UserId
            };
        } 

        public static HeroAssignDocument AsDocument(this HeroAssign hero)
        {
            return new HeroAssignDocument
            {
                Id = hero.Id,
                HeroName = hero.HeroName,
                Attack = hero.Attack,
                BaseRequiredExperience = hero.BaseRequiredExperience,
                HealLvl = hero.HealLvl,
                Health = hero.Health,
                Skills = hero.Skills.Select(s => s.AsDocument()),
            };
        }

        public static SkillHeroAssignDocument AsDocument(this SkillHeroAssign skill)
        {
            return new SkillHeroAssignDocument
            {
                Id = skill.Id,
                Name = skill.Name,
                BaseAttack = skill.BaseAttack,
            };
        }

        public static Player AsEntity(this PlayerDocument playerDocument)
        {
            return new Player
            {
                Id = playerDocument.Id,
                Name = playerDocument.Name,
                CurrentExp = playerDocument.CurrentExp,
                Hero = playerDocument.Hero.AsEntity(),
                Level = playerDocument.Level,
                RequiredExp = playerDocument.RequiredExp,
                UserId = playerDocument.UserId
            };
        }

        public static HeroAssign AsEntity(this HeroAssignDocument heroAssignDocument)
        {
            return new HeroAssign
            {
                Id = heroAssignDocument.Id,
                HeroName = heroAssignDocument.HeroName,
                Attack = heroAssignDocument.Attack,
                BaseRequiredExperience = heroAssignDocument.BaseRequiredExperience,
                HealLvl = heroAssignDocument.HealLvl,
                Health = heroAssignDocument.Health,
                Skills = heroAssignDocument.Skills.Select(s => s.AsEntity())
            };
        }

        public static SkillHeroAssign AsEntity(this SkillHeroAssignDocument skillHeroAssignDocument)
        {
            return new SkillHeroAssign
            {
                Id = skillHeroAssignDocument.Id,
                Name = skillHeroAssignDocument.Name,
                BaseAttack = skillHeroAssignDocument.BaseAttack
            };
        }
        
        public static HeroDocument AsDocument(this Hero hero)
        {
            return new HeroDocument
            {
                Id = hero.Id,
                HeroName = hero.HeroName,
                Attack = hero.Attack.AsDocument(),
                HealLvl = hero.HealLvl.AsDocument(),
                Health = hero.Health.AsDocument(),
                BaseRequiredExperience = hero.BaseRequiredExperience.AsDocument(),
                Skills = hero.Skills.Select(s => s.AsDocument()),
                PlayersAssignedTo = hero.PlayersAssignedTo
            };
        }

        public static StateDocument<T> AsDocument<T>(this State<T> state)
            where T : struct
        {
            return new StateDocument<T>
            {
                IncreasingStats = state.IncreasingStats.AsDocument(),
                Value = state.Value
            };
        }

        public static IncreasingStateDocument<T> AsDocument<T>(this IncreasingState<T> increasingState)
            where T : struct
        {
            return new IncreasingStateDocument<T>
            {
                StrategyIncreasing = Enum.Parse<Documents.StrategyIncreasing>(increasingState.StrategyIncreasing.ToString()),
                Value = increasingState.Value
            };
        }

        public static SkillHeroDocument AsDocument(this SkillHero skillHero)
        {
            return new SkillHeroDocument
            {
                Id = skillHero.Id,
                Name = skillHero.Name,
                BaseAttack = skillHero.BaseAttack,
                IncreasingStats = skillHero.IncreasingStats.AsDocument()
            };
        }

        public static Hero AsEntity(this HeroDocument heroDocument)
        {
            return new Hero
            {
                Id = heroDocument.Id,
                HeroName = heroDocument.HeroName,
                Attack = heroDocument.Attack.AsEntity(),
                HealLvl = heroDocument.HealLvl.AsEntity(),
                Health = heroDocument.Health.AsEntity(),
                BaseRequiredExperience = heroDocument.BaseRequiredExperience.AsEntity(),
                Skills = heroDocument.Skills.Select(s => s.AsEntity()),
                PlayersAssignedTo = heroDocument.PlayersAssignedTo
            };
        }

        public static State<T> AsEntity<T>(this StateDocument<T> stateDocument)
            where T : struct
        {
            return new State<T>
            {
                Value = stateDocument.Value,
                IncreasingStats = stateDocument.IncreasingStats.AsEntity()
            };
        }

        public static IncreasingState<T> AsEntity<T>(this IncreasingStateDocument<T> increasingStateDocument)
            where T : struct
        {
            return new IncreasingState<T>
            {
                Value = increasingStateDocument.Value,
                StrategyIncreasing = Enum.Parse<RPG_GAME.Core.Entities.Common.StrategyIncreasing>(increasingStateDocument.StrategyIncreasing.ToString()),
            };
        }

        public static SkillHero AsEntity(this SkillHeroDocument skillHeroDocument)
        {
            return new SkillHero
            {
                Id = skillHeroDocument.Id,
                Name = skillHeroDocument.Name,
                BaseAttack = skillHeroDocument.BaseAttack,
                IncreasingStats = skillHeroDocument.IncreasingStats.AsEntity()
            };
        }

        public static EnemyDocument AsDocument(this Enemy enemy)
        {
            return new EnemyDocument
            {
                Id = enemy.Id,
                EnemyName = enemy.EnemyName,
                BaseAttack = enemy.BaseAttack.AsDocument(),
                BaseHealLvl = enemy.BaseHealLvl.AsDocument(),
                BaseHealth = enemy.BaseHealth.AsDocument(),
                Difficulty = Enum.Parse<Documents.Difficulty>(enemy.Difficulty.ToString()),
                Experience = enemy.Experience.AsDocument(),
                Skills = enemy.Skills.Select(s => s.AsDocument()),
                MapsAssignedTo = enemy.MapsAssignedTo
            };
        }

        public static SkillEnemyDocument AsDocument(this SkillEnemy skillEnemy)
        {
            return new SkillEnemyDocument
            {
                Id = skillEnemy.Id,
                Name = skillEnemy.Name,
                BaseAttack = skillEnemy.BaseAttack,
                IncreasingStats = skillEnemy.IncreasingStats.AsDocument(),
                Probability = skillEnemy.Probability
            };
        }

        public static Enemy AsEntity(this EnemyDocument enemyDocument)
        {
            return new Enemy
            {
                Id = enemyDocument.Id,
                EnemyName = enemyDocument.EnemyName,
                BaseAttack = enemyDocument.BaseAttack.AsEntity(),
                BaseHealLvl = enemyDocument.BaseHealLvl.AsEntity(),
                BaseHealth = enemyDocument.BaseHealth.AsEntity(),
                Difficulty = Enum.Parse<RPG_GAME.Core.Entities.Common.Difficulty>(enemyDocument.Difficulty.ToString()),
                Experience = enemyDocument.Experience.AsEntity(),
                Skills = enemyDocument.Skills.Select(s => s.AsEntity()),
                MapsAssignedTo = enemyDocument.MapsAssignedTo
            };
        }

        public static SkillEnemy AsEntity(this SkillEnemyDocument skillEnemyDocument)
        {
            return new SkillEnemy
            {
                Id = skillEnemyDocument.Id,
                Name = skillEnemyDocument.Name,
                BaseAttack = skillEnemyDocument.BaseAttack,
                Probability = skillEnemyDocument.Probability,
                IncreasingStats = skillEnemyDocument.IncreasingStats.AsEntity()
            };
        }

        public static MapDocument AsDocument(this Map map)
        {
            return new MapDocument
            {
                Id = map.Id,
                Name = map.Name,
                Difficulty = Enum.Parse<Documents.Difficulty>(map.Difficulty.ToString()),
                Enemies = map.Enemies.Select(e => e.AsDocument())
            };
        }

        public static Map AsEntity(this MapDocument mapDocument)
        {
            return new Map
            {
                Id = mapDocument.Id,
                Name = mapDocument.Name,
                Difficulty = Enum.Parse<RPG_GAME.Core.Entities.Common.Difficulty>(mapDocument.Difficulty.ToString()),
                Enemies = mapDocument.Enemies.Select(e => e.AsEntity()).ToList()
            };
        }

        public static EnemiesDocument AsDocument(this Enemies enemy)
        {
            return new EnemiesDocument
            {
                Enemy = enemy.Enemy.AsDocument(),
                Quantity = enemy.Quantity
            };
        }

        public static EnemyAssignDocument AsDocument(this EnemyAssign enemy)
        {
            return new EnemyAssignDocument
            {
                Id = enemy.Id,
                EnemyName = enemy.EnemyName,
                BaseAttack = enemy.BaseAttack,
                BaseHealLvl = enemy.BaseHealLvl,
                BaseHealth = enemy.BaseHealth,
                Experience = enemy.Experience,
                Difficulty = Enum.Parse<Documents.Difficulty>(enemy.Difficulty.ToString()),
                Skills = enemy.Skills.Select(s => s.AsDocument()),
            };
        }

        public static SkillEnemyAssignDocument AsDocument(this SkillEnemyAssign skillEnemy)
        {
            return new SkillEnemyAssignDocument
            {
                Id = skillEnemy.Id,
                Name = skillEnemy.Name,
                BaseAttack = skillEnemy.BaseAttack,
                Probability = skillEnemy.Probability
            };
        }

        public static Enemies AsEntity(this EnemiesDocument enemiesDocument)
        {
            return new Enemies
            {
                Enemy = enemiesDocument.Enemy.AsEntity(),
                Quantity = enemiesDocument.Quantity
            };
        }

        public static EnemyAssign AsEntity(this EnemyAssignDocument enemyDocument)
        {
            return new EnemyAssign
            {
                Id = enemyDocument.Id,
                EnemyName = enemyDocument.EnemyName,
                BaseAttack = enemyDocument.BaseAttack,
                BaseHealLvl = enemyDocument.BaseHealLvl,
                BaseHealth = enemyDocument.BaseHealth,
                Experience = enemyDocument.Experience,
                Difficulty = Enum.Parse<RPG_GAME.Core.Entities.Common.Difficulty>(enemyDocument.Difficulty.ToString()),
                Skills = enemyDocument.Skills.Select(s => s.AsEntity()),
            };
        }

        public static SkillEnemyAssign AsEntity(this SkillEnemyAssignDocument skillEnemyDocument)
        {
            return new SkillEnemyAssign
            {
                Id = skillEnemyDocument.Id,
                Name = skillEnemyDocument.Name,
                BaseAttack = skillEnemyDocument.BaseAttack,
                Probability = skillEnemyDocument.Probability
            };
        }
    }
}
