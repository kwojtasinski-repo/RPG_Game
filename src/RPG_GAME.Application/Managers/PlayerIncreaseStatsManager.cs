using Microsoft.Extensions.Logging;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Managers
{
    internal sealed class PlayerIncreaseStatsManager : IPlayerIncreaseStatsManager
    {
        private readonly ILogger<PlayerIncreaseStatsManager> _logger;

        public PlayerIncreaseStatsManager(ILogger<PlayerIncreaseStatsManager> logger)
        {
            _logger = logger;
        }

        public void CalculateHeroStats(int level, HeroAssign heroAssign, Hero hero)
        {
            heroAssign.ChangeAttack(CalculateStats(hero.Attack.Value, hero.Attack.IncreasingState.Value, hero.Attack.IncreasingState.StrategyIncreasing, level));
            heroAssign.ChangeHealth(CalculateStats(hero.Health.Value, hero.Health.IncreasingState.Value, hero.Health.IncreasingState.StrategyIncreasing, level));
            CalculateHeroSkills(level, heroAssign, hero.Skills);
        }

        public void CalculatePlayerStats(Player player, Hero hero)
        {
            player.ChangeRequiredExp(CalculateStats(player.RequiredExp, hero.BaseRequiredExperience.IncreasingState.Value, hero.BaseRequiredExperience.IncreasingState.StrategyIncreasing, player.Level));
            CalculateHeroStats(player.Level, player.Hero, hero);
        }

        private int CalculateStats(int stats, int increasingValue, StrategyIncreasing strategyIncreasing, int level)
        {
            switch (strategyIncreasing)
            {
                case StrategyIncreasing.PERCENTAGE:
                    int value = stats;
                    for (int i = 1; i < level; i++)
                    {
                        var increasedValue = (int)Math.Ceiling(stats * (increasingValue / 100.0));
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
        
        private decimal CalculateStats(decimal stats, decimal increasingValue, StrategyIncreasing strategyIncreasing, int level)
        {
            switch (strategyIncreasing)
            {
                case StrategyIncreasing.PERCENTAGE:
                    decimal value = stats;
                    for (int i = 1; i < level; i++)
                    {
                        var increasedValue = stats * (increasingValue / 100M);
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

        public void CalculateHeroSkills(int level, HeroAssign heroAssign, IEnumerable<SkillHero> skills)
        {
            foreach (var skill in heroAssign.Skills)
            {
                var skillHero = skills.SingleOrDefault(s => s.Id == skill.Id);

                if (skillHero is null)
                {
                    _logger.LogError($"Hero with id '{heroAssign.Id}' and name '{heroAssign.HeroName}' dont have skill '{skill.Name}'");
                    continue;
                }

                skill.ChangeAttack(CalculateStats(skill.Attack, skillHero.IncreasingState.Value, skillHero.IncreasingState.StrategyIncreasing, level));
            }
        }
    }
}
