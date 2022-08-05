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
            return new User(
                userDocument.Id,
                userDocument.Email,
                userDocument.Password,
                userDocument.Role,
                userDocument.CreatedAt,
                userDocument.IsActive
            );
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
                BaseAttack = skill.Attack,
            };
        }

        public static Player AsEntity(this PlayerDocument playerDocument)
        {
            return new Player(
                playerDocument.Id,
                playerDocument.Name,
                playerDocument.Hero.AsEntity(),
                playerDocument.Level,
                playerDocument.CurrentExp,
                playerDocument.RequiredExp,
                playerDocument.UserId
            );
        }

        public static HeroAssign AsEntity(this HeroAssignDocument heroAssignDocument)
        {
            return new HeroAssign(
                heroAssignDocument.Id,
                heroAssignDocument.HeroName,
                heroAssignDocument.Health,
                heroAssignDocument.Attack,
                heroAssignDocument.HealLvl,
                heroAssignDocument.Skills.Select(s => s.AsEntity())
            );
        }

        public static SkillHeroAssign AsEntity(this SkillHeroAssignDocument skillHeroAssignDocument)
        {
            return new SkillHeroAssign(
                skillHeroAssignDocument.Id,
                skillHeroAssignDocument.Name,
                skillHeroAssignDocument.BaseAttack
            );
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
                IncreasingState = state.IncreasingState.AsDocument(),
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
                IncreasingState = skillHero.IncreasingState.AsDocument()
            };
        }

        public static Hero AsEntity(this HeroDocument heroDocument)
        {
            return new Hero(heroDocument.Id, heroDocument.HeroName,
                    heroDocument.Health.AsEntity(),
                    heroDocument.Attack.AsEntity(), 
                    heroDocument.HealLvl.AsEntity(), 
                    heroDocument.BaseRequiredExperience.AsEntity(),
                    heroDocument.Skills.Select(s => s.AsEntity()), 
                    heroDocument.PlayersAssignedTo);
        }

        public static State<T> AsEntity<T>(this StateDocument<T> stateDocument)
            where T : struct
        {
            return new State<T>(stateDocument.Value, stateDocument.IncreasingState.AsEntity());
        }

        public static IncreasingState<T> AsEntity<T>(this IncreasingStateDocument<T> increasingStateDocument)
            where T : struct
        {
            return new IncreasingState<T>(increasingStateDocument.Value, increasingStateDocument.StrategyIncreasing.ToString());
        }

        public static SkillHero AsEntity(this SkillHeroDocument skillHeroDocument)
        {
            return new SkillHero(skillHeroDocument.Id, skillHeroDocument.Name, skillHeroDocument.BaseAttack, skillHeroDocument.IncreasingState.AsEntity());
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
                IncreasingState = skillEnemy.IncreasingState.AsDocument(),
                Probability = skillEnemy.Probability
            };
        }

        public static Enemy AsEntity(this EnemyDocument enemyDocument)
        {
            return new Enemy(
                enemyDocument.Id,
                enemyDocument.EnemyName,
                enemyDocument.BaseHealth.AsEntity(),
                enemyDocument.BaseAttack.AsEntity(),
                enemyDocument.BaseHealLvl.AsEntity(),
                enemyDocument.Experience.AsEntity(),
                enemyDocument.Difficulty.ToString(),
                enemyDocument.Skills.Select(s => s.AsEntity()),
                enemyDocument.MapsAssignedTo);
        }

        public static SkillEnemy AsEntity(this SkillEnemyDocument skillEnemyDocument)
        {
            return new SkillEnemy(
                skillEnemyDocument.Id,
                skillEnemyDocument.Name,
                skillEnemyDocument.BaseAttack,
                skillEnemyDocument.Probability,
                skillEnemyDocument.IncreasingState.AsEntity()
            );
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
            return new Map(
                mapDocument.Id,
                mapDocument.Name,
                mapDocument.Difficulty.ToString(),
                mapDocument.Enemies.Select(e => e.AsEntity()).ToList()
            );
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
                BaseAttack = enemy.Attack,
                BaseHealLvl = enemy.HealLvl,
                BaseHealth = enemy.Health,
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
                BaseAttack = skillEnemy.Attack,
                Probability = skillEnemy.Probability
            };
        }

        public static Enemies AsEntity(this EnemiesDocument enemiesDocument)
        {
            return new Enemies(
                enemiesDocument.Enemy.AsEntity(),
                enemiesDocument.Quantity
            );
        }

        public static EnemyAssign AsEntity(this EnemyAssignDocument enemyDocument)
        {
            return new EnemyAssign(
                enemyDocument.Id,
                enemyDocument.EnemyName,
                enemyDocument.BaseAttack,
                enemyDocument.BaseHealth,
                enemyDocument.BaseHealLvl,
                enemyDocument.Experience,
                enemyDocument.Difficulty.ToString(),
                enemyDocument.Skills.Select(s => s.AsEntity())
            );
        }

        public static SkillEnemyAssign AsEntity(this SkillEnemyAssignDocument skillEnemyDocument)
        {
            return new SkillEnemyAssign(
                skillEnemyDocument.Id,
                skillEnemyDocument.Name,
                skillEnemyDocument.BaseAttack,
                skillEnemyDocument.Probability
            );
        }
    }
}
