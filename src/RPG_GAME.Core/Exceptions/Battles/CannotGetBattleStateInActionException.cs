using RPG_GAME.Core.Entities.Battles;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class CannotGetBattleStateInActionException : DomainException
    {
        public BattleInfo BattleInfo { get; }

        public CannotGetBattleStateInActionException(BattleInfo battleInfo) : base($"Cannot get BattleState with status 'InAction' for battle with info state '{battleInfo}'")
        {
            BattleInfo = battleInfo;
        }
    }
}
