using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Managers
{
    public interface IBattleManager
    {
        Task<Player> CompleteBattle(Battle battle, Player player);
        Task<BattleEvent> CreateBattleEvent(Guid battleId, Player player, string attackInfo);
    }
}
