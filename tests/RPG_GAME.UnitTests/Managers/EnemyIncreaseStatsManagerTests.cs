using FluentAssertions;
using RPG_GAME.Application.Managers;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Common;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.UnitTests.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RPG_GAME.UnitTests.Managers
{
    public class EnemyIncreaseStatsManagerTests
    {
        [Fact]
        public void should_increase_enemy_stats()
        {
            var skillEnemy = new SkillEnemy(Guid.NewGuid(), "skill", 1000, 10, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString()));
            var enemy = new Enemy(Guid.NewGuid(), "enemy", new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString())), new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString())),
                    new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.PERCENTAGE.ToString())), new State<decimal>(1000, new IncreasingState<decimal>(10, StrategyIncreasing.PERCENTAGE.ToString())),
                    Difficulty.EASY.ToString(), Category.Knight.ToString(), new List<SkillEnemy> { skillEnemy });
            var enemyAssign = enemy.AsAssign();
            var level = 5;
            var calculatedAttack = 140;
            var calculatedHealth = 140;
            var calculatedHeal = 140;
            var calculatedSkillAttack = 1400;
            var calculatedExp = 1400;

            _enemyIncreaseStatsManager.IncreaseEnemyStats(level, enemyAssign, enemy);

            var skillAfterCalculate = enemyAssign.Skills.Single(s => s.Id == skillEnemy.Id);
            skillAfterCalculate.Attack.Should().Be(calculatedSkillAttack);
            enemyAssign.Attack.Should().Be(calculatedAttack);
            enemyAssign.Health.Should().Be(calculatedHealth);
            enemyAssign.HealLvl.Should().Be(calculatedHeal);
            enemyAssign.Experience.Should().Be(calculatedExp);
        }

        private readonly IEnemyIncreaseStatsManager _enemyIncreaseStatsManager;

        public EnemyIncreaseStatsManagerTests()
        {
            _enemyIncreaseStatsManager = new EnemyIncreaseStatsManager(LoggerStub<EnemyIncreaseStatsManager>.Create());
        }
    }
}
