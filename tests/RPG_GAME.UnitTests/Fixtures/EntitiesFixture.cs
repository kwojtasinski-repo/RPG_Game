using RPG_GAME.Application.DTO.Common;
using RPG_GAME.Application.DTO.Enemies;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.UnitTests.Fixtures
{
    internal static class EntitiesFixture
    {
        public static Hero CreateDefaultHero()
        {
            return new Hero(Guid.NewGuid(), "Hero", new State<int>(100, DefaultIncreasingState<int>()), new State<int>(100, DefaultIncreasingState<int>()),
                    new State<int>(10, DefaultIncreasingState<int>()), new State<decimal>(1000, DefaultIncreasingState<decimal>()));
        }

        public static Enemy CreateDefaultEnemy()
        {
            return new Enemy(Guid.NewGuid(), "Enemy", new State<int>(100, DefaultIncreasingState<int>()), new State<int>(100, DefaultIncreasingState<int>()),
                    new State<int>(10, DefaultIncreasingState<int>()), new State<decimal>(1000, DefaultIncreasingState<decimal>()),
                    "EASY", "Knight");
        }

        public static IncreasingState<T> DefaultIncreasingState<T>()
           where T : struct
        {
            T value = default;

            if (typeof(T) == typeof(decimal))
            {
                object val = 1M;
                value = (T)val;
            }

            if (typeof(T) == typeof(int))
            {
                object val = 1;
                value = (T)val;
            }

            return new IncreasingState<T>(value, "ADDITIVE");
        }

        public static SkillHero CreateDefaultSkillHero(string name = null, int? baseAttack = null)
        {
            return new SkillHero(Guid.NewGuid(), name ?? "skill" + Guid.NewGuid().ToString("N"), baseAttack ?? 10, DefaultIncreasingState());
        }

        public static SkillEnemy CreateDefaultSkillEnemy(string name = null, int? baseAttack = null, decimal? probability = null)
        {
            return new SkillEnemy(Guid.NewGuid(), name ?? "skill" + Guid.NewGuid().ToString("N"), baseAttack ?? 10, probability ?? 20, DefaultIncreasingState());
        }

        public static IncreasingState<int> DefaultIncreasingState()
        {
            return new IncreasingState<int>(1, "ADDITIVE");
        }

        public static Hero Clone(Hero hero)
        {
            return new Hero(hero.Id, hero.HeroName, new State<int>(hero.Health.Value, new IncreasingState<int>(hero.Health.IncreasingState.Value, hero.Health.IncreasingState.StrategyIncreasing.ToString())),
                new State<int>(hero.Attack.Value, new IncreasingState<int>(hero.Attack.IncreasingState.Value, hero.Attack.IncreasingState.StrategyIncreasing.ToString())), new State<int>(hero.HealLvl.Value, new IncreasingState<int>(hero.HealLvl.IncreasingState.Value, hero.HealLvl.IncreasingState.StrategyIncreasing.ToString())),
                new State<decimal>(hero.BaseRequiredExperience.Value, new IncreasingState<decimal>(hero.BaseRequiredExperience.IncreasingState.Value, hero.BaseRequiredExperience.IncreasingState.StrategyIncreasing.ToString())),
                hero.Skills.Select(s => new SkillHero(s.Id, s.Name, s.BaseAttack, new IncreasingState<int>(s.IncreasingState.Value, s.IncreasingState.StrategyIncreasing.ToString()))),
                hero.PlayersAssignedTo.Select(p => p));
        }

        public static HeroDto Clone(HeroDto heroDto)
        {
            return new HeroDto
            {
                Id = heroDto.Id,
                HeroName = heroDto.HeroName,
                Attack = new StateDto<int> { Value = heroDto.Attack.Value, IncreasingState = new IncreasingStateDto<int> { Value = heroDto.Attack.IncreasingState.Value, StrategyIncreasing = heroDto.Attack.IncreasingState.StrategyIncreasing.ToString() } },
                Health = new StateDto<int> { Value = heroDto.Health.Value, IncreasingState = new IncreasingStateDto<int> { Value = heroDto.Health.IncreasingState.Value, StrategyIncreasing = heroDto.Health.IncreasingState.StrategyIncreasing.ToString() } },
                HealLvl = new StateDto<int> { Value = heroDto.HealLvl.Value, IncreasingState = new IncreasingStateDto<int> { Value = heroDto.HealLvl.IncreasingState.Value, StrategyIncreasing = heroDto.HealLvl.IncreasingState.StrategyIncreasing.ToString() } },
                BaseRequiredExperience = new StateDto<decimal> { Value = heroDto.BaseRequiredExperience.Value, IncreasingState = new IncreasingStateDto<decimal> { Value = heroDto.BaseRequiredExperience.IncreasingState.Value, StrategyIncreasing = heroDto.BaseRequiredExperience.IncreasingState.StrategyIncreasing.ToString() } },
                Skills = heroDto.Skills,
            };
        }

        public static Enemy Clone(Enemy enemy)
        {
            return new Enemy(enemy.Id, enemy.EnemyName, new State<int>(enemy.BaseHealth.Value, new IncreasingState<int>(enemy.BaseHealth.IncreasingState.Value, enemy.BaseHealth.IncreasingState.StrategyIncreasing.ToString())),
                new State<int>(enemy.BaseAttack.Value, new IncreasingState<int>(enemy.BaseAttack.IncreasingState.Value, enemy.BaseAttack.IncreasingState.StrategyIncreasing.ToString())), new State<int>(enemy.BaseHealLvl.Value, new IncreasingState<int>(enemy.BaseHealLvl.IncreasingState.Value, enemy.BaseHealLvl.IncreasingState.StrategyIncreasing.ToString())),
                new State<decimal>(enemy.Experience.Value, new IncreasingState<decimal>(enemy.Experience.IncreasingState.Value, enemy.Experience.IncreasingState.StrategyIncreasing.ToString())),
                enemy.Difficulty.ToString(), enemy.Category.ToString(), enemy.Skills.Select(s => new SkillEnemy(s.Id, s.Name, s.BaseAttack, s.Probability, new IncreasingState<int>(s.IncreasingState.Value, s.IncreasingState.StrategyIncreasing.ToString()))),
                enemy.MapsAssignedTo.Select(m => m));
        }

        public static EnemyDto Clone(EnemyDto enemyDto)
        {
            return new EnemyDto
            {
                Id = enemyDto.Id,
                EnemyName = enemyDto.EnemyName,
                BaseAttack = new StateDto<int> { Value = enemyDto.BaseAttack.Value, IncreasingState = new IncreasingStateDto<int> { Value = enemyDto.BaseAttack.IncreasingState.Value, StrategyIncreasing = enemyDto.BaseAttack.IncreasingState.StrategyIncreasing.ToString() } },
                BaseHealth = new StateDto<int> { Value = enemyDto.BaseHealth.Value, IncreasingState = new IncreasingStateDto<int> { Value = enemyDto.BaseHealth.IncreasingState.Value, StrategyIncreasing = enemyDto.BaseHealth.IncreasingState.StrategyIncreasing.ToString() } },
                BaseHealLvl = new StateDto<int> { Value = enemyDto.BaseHealLvl.Value, IncreasingState = new IncreasingStateDto<int> { Value = enemyDto.BaseHealLvl.IncreasingState.Value, StrategyIncreasing = enemyDto.BaseHealLvl.IncreasingState.StrategyIncreasing.ToString() } },
                Experience = new StateDto<decimal> { Value = enemyDto.Experience.Value, IncreasingState = new IncreasingStateDto<decimal> { Value = enemyDto.Experience.IncreasingState.Value, StrategyIncreasing = enemyDto.Experience.IncreasingState.StrategyIncreasing.ToString() } },
                Difficulty = enemyDto.Difficulty,
                Skills = enemyDto.Skills,
            };
        }

        public static Enemies CreateEnemies(EnemyAssign enemyAssign, int quantity = 1)
        {
            return new Enemies(enemyAssign, quantity);
        }

        public static Map CreateDefaultMap(string name = null, string difficulty = null, IEnumerable<Enemies> enemies = null)
        {
            return Map.Create(name ?? "Map#1", difficulty ?? "EASY", enemies);
        }
    }
}
