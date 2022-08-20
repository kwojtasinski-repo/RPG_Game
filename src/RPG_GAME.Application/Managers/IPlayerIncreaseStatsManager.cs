using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Managers
{
    public interface IPlayerIncreaseStatsManager
    {
        void IncreaseHeroStats(int level, HeroAssign heroAssign, Hero hero);
        void IncreasePlayerStats(Player player, Hero hero);
    }
}
