using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Managers
{
    internal sealed class PlayerIncreaseStatsManager : IPlayerIncreaseStatsManager
    {
        public void IncreaseHeroStats(int level, HeroAssign heroAssign, Hero hero)
        {
            heroAssign.ChangeAttack(CalculateStats(hero.Attack.Value, hero.Attack.IncreasingState.Value, hero.Attack.IncreasingState.StrategyIncreasing, level));
            heroAssign.ChangeHealLvl(CalculateStats(hero.HealLvl.Value, hero.HealLvl.IncreasingState.Value, hero.HealLvl.IncreasingState.StrategyIncreasing, level));
            heroAssign.ChangeHealth(CalculateStats(hero.Health.Value, hero.Health.IncreasingState.Value, hero.Health.IncreasingState.StrategyIncreasing, level));

            foreach(var skill in heroAssign.Skills)
            {
                var skillHero = hero.Skills.SingleOrDefault(s => s.Id == skill.Id);

                if (skillHero is null)
                {
                    // TODO: inform maybe logs?
                    continue;
                }

                skill.ChangeAttack(CalculateStats(skill.Attack, skillHero.IncreasingState.Value, skillHero.IncreasingState.StrategyIncreasing, level));
            }
        }

        public void IncreasePlayerStats(Player player, Hero hero)
        {
            player.ChangeRequiredExp(CalculateStats(player.RequiredExp, hero.BaseRequiredExperience.IncreasingState.Value, hero.BaseRequiredExperience.IncreasingState.StrategyIncreasing, player.Level));
            IncreaseHeroStats(player.Level, player.Hero, hero);
        }

        public int CalculateStats(int stats, int increasingValue, StrategyIncreasing strategyIncreasing, int level)
        {
            switch (strategyIncreasing)
            {
                case StrategyIncreasing.PERCENTAGE:
                    int value = 0;
                    for (int i = 1; i <= level; i++)
                    {
                        var increasedValue = (int)Math.Ceiling(increasingValue * stats / 100.0);
                        value += increasedValue + value;
                    }

                    return value;
                case StrategyIncreasing.ADDITIVE:
                    value = 0;
                    for (int i = 1; i <= level; i++)
                    {
                        value += increasingValue + value;
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
                    decimal value = 0;
                    for (int i = 1; i <= level; i++)
                    {
                        var increasedValue = increasingValue * stats / 100;
                        value += increasedValue + value;
                    }

                    return value;
                case StrategyIncreasing.ADDITIVE:
                    value = 0;
                    for (int i = 1; i <= level; i++)
                    {
                        value += increasingValue + value;
                    }

                    return value;
                default:
                    return stats;
            }
        }
    }
}
