using Microsoft.Extensions.Logging;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Maps;

namespace RPG_GAME.Application.Managers
{
    internal sealed class EnemyIncreaseStatsManager : IEnemyIncreaseStatsManager
    {
        private readonly ILogger<EnemyIncreaseStatsManager> _logger;

        public EnemyIncreaseStatsManager(ILogger<EnemyIncreaseStatsManager> logger)
        {
            _logger = logger;
        }

        public void IncreaseEnemyStats(int level, EnemyAssign enemyAssign, Enemy enemy)
        {
            enemyAssign.ChangeAttack(CalculateStats(enemy.BaseAttack.Value, enemy.BaseAttack.IncreasingState.Value, enemy.BaseAttack.IncreasingState.StrategyIncreasing, level));
            enemyAssign.ChangeHealLvl(CalculateStats(enemy.BaseHealLvl.Value, enemy.BaseHealLvl.IncreasingState.Value, enemy.BaseHealLvl.IncreasingState.StrategyIncreasing, level));
            enemyAssign.ChangeHealth(CalculateStats(enemy.BaseHealth.Value, enemy.BaseHealth.IncreasingState.Value, enemy.BaseHealth.IncreasingState.StrategyIncreasing, level));
            enemyAssign.ChangeExperience(CalculateStats(enemy.Experience.Value, enemy.Experience.IncreasingState.Value, enemy.Experience.IncreasingState.StrategyIncreasing, level));

            foreach (var skill in enemyAssign.Skills)
            {
                var skillEnemy = enemy.Skills.SingleOrDefault(s => s.Id == skill.Id);

                if (skillEnemy is null)
                {
                    _logger.LogError($"Enemy with id '{enemy.Id}' and name '{enemy.EnemyName}' dont have skill '{skill.Name}'");
                    continue;
                }

                skill.ChangeAttack(CalculateStats(skill.Attack, skillEnemy.IncreasingState.Value, skillEnemy.IncreasingState.StrategyIncreasing, level));
                enemyAssign.ChangeSkillAttack(skill);
            }

        }

        public int CalculateStats(int stats, int increasingValue, StrategyIncreasing strategyIncreasing, int level)
        {
            switch (strategyIncreasing)
            {
                case StrategyIncreasing.PERCENTAGE:
                    int value = stats;
                    for (int i = 1; i < level; i++)
                    {
                        var increasedValue = (int)Math.Ceiling(increasingValue * stats / 100.0);
                        value += increasedValue;
                    }

                    return value;
                case StrategyIncreasing.ADDITIVE:
                    value = stats;
                    for (int i = 1; i < level; i++)
                    {
                        value += increasingValue;
                    }

                    return value;
                default:
                    return stats;
            }
        }

        public decimal CalculateStats(decimal stats, decimal increasingValue, StrategyIncreasing strategyIncreasing, int level)
        {
            switch (strategyIncreasing)
            {
                case StrategyIncreasing.PERCENTAGE:
                    decimal value = stats;
                    for (int i = 1; i < level; i++)
                    {
                        var increasedValue = increasingValue * stats / 100;
                        value += increasedValue;
                    }

                    return value;
                case StrategyIncreasing.ADDITIVE:
                    value = stats;
                    for (int i = 1; i < level; i++)
                    {
                        value += increasingValue;
                    }

                    return value;
                default:
                    return stats;
            }
        }
    }
}
