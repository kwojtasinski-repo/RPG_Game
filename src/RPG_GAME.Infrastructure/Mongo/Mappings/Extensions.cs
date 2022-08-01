using RPG_Game.Infrastructure.Mongo.Documents;
using RPG_GAME.Core.Entities;

namespace RPG_Game.Infrastructure.Mongo.Mappings
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
        
        public static HeroDocument AsDocument(this Hero hero)
        {
            return new HeroDocument
            {
                Id = hero.Id,
                HeroName = hero.HeroName,
                Attack = hero.Attack,
                HealLvl = hero.HealLvl,
                Health = hero.Health,
                BaseRequiredExperience = hero.BaseRequiredExperience,
                Character = hero.Character,
                Skills = hero.Skills,
            };
        }

        public static Hero AsEntity(this HeroDocument heroDocument)
        {
            return new Hero
            {
                Id = heroDocument.Id,
                HeroName = heroDocument.HeroName,
                Attack = heroDocument.Attack,
                HealLvl = heroDocument.HealLvl,
                Health = heroDocument.Health,
                BaseRequiredExperience = heroDocument.BaseRequiredExperience,
                Skills = heroDocument.Skills,
            };
        }

        public static EnemyDocument AsDocument(this Enemy enemy)
        {
            return new EnemyDocument
            {
                Id = enemy.Id,
                BaseAttack = enemy.BaseAttack,
                BaseHealLvl = enemy.BaseHealLvl,
                BaseHealth = enemy.BaseHealth,
                Character = enemy.Character,
                Difficulty = enemy.Difficulty,
                Experience = enemy.Experience,
                EnemyName = enemy.EnemyName,
                Skills = enemy.Skills
            };
        }

        public static Enemy AsEntity(this EnemyDocument enemyDocument)
        {
            return new Enemy
            {
                Id = enemyDocument.Id,
                BaseAttack = enemyDocument.BaseAttack,
                BaseHealLvl = enemyDocument.BaseHealLvl,
                BaseHealth = enemyDocument.BaseHealth,
                Difficulty = enemyDocument.Difficulty,
                Experience = enemyDocument.Experience,
                EnemyName = enemyDocument.EnemyName,
                Skills = enemyDocument.Skills
            };
        }

        public static MapDocument AsDocument(this Map map)
        {
            return new MapDocument
            {
                Id = map.Id,
                Name = map.Name,
                Difficulty = map.Difficulty,
                Enemies = map.Enemies.Select(e => e.AsDocument()).ToList()
            };
        }

        public static Map AsEntity(this MapDocument mapDocument)
        {
            return new Map
            {
                Id = mapDocument.Id,
                Name = mapDocument.Name,
                Difficulty = mapDocument.Difficulty,
                Enemies = mapDocument.Enemies.Select(e => e.AsEntity()).ToList()
            };
        }

        public static RequiredEnemyDocument AsDocument(this RequiredEnemy enemy)
        {
            return new RequiredEnemyDocument
            {
                Enemy = enemy.Enemy.AsDocument(),
                Quantity = enemy.Quantity
            };
        }

        public static RequiredEnemy AsEntity(this RequiredEnemyDocument enemyDocument)
        {
            return new RequiredEnemy
            {
                Enemy = enemyDocument.Enemy.AsEntity(),
                Quantity = enemyDocument.Quantity
            };
        }
    }
}
