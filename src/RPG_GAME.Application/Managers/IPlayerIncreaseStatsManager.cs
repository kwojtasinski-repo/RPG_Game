using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Managers
{
    public interface IPlayerIncreaseStatsManager
    {
        void CalculateHeroStats(int level, HeroAssign heroAssign, Hero hero);
        void CalculatePlayerStats(Player player, Hero hero);
        void CalculateHeroSkills(int level, HeroAssign hero, IEnumerable<SkillHero> skills);
    }
}
