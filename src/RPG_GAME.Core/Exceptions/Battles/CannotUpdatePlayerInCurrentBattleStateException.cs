using RPG_GAME.Core.Entities.Battles;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal class CannotUpdatePlayerInCurrentBattleStateException : DomainException
    {
        public BattleStatus BattleStatus { get; }

        public CannotUpdatePlayerInCurrentBattleStateException(BattleStatus battleStatus) : base($"Cannot update Player in current BattleState '{battleStatus}'")
        {
            BattleStatus = battleStatus;
        }
    }
}
