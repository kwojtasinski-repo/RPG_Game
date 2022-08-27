using RPG_GAME.Core.Entities.Battles;

namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class CannotStartBattleForBattleInfoException : BusinessException
    {
        public BattleInfo BattleInfo { get; }

        public CannotStartBattleForBattleInfoException(BattleInfo battleInfo) : base($"Cannot start Battle for BattleInfo '{battleInfo}'")
        {
            BattleInfo = battleInfo;
        }
    }
}
