using RPG_GAME.Core.Entities.Battles;

namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class CannotAddBattleEventForBattleInfoException : BusinessException
    {
        public BattleInfo BattleInfo { get; }

        public CannotAddBattleEventForBattleInfoException(BattleInfo battleInfo) : base($"Cannot add BattleEvent for battle with BattleInfo '{battleInfo}'")
        {
            BattleInfo = battleInfo;
        }
    }
}
