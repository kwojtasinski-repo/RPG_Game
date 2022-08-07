using RPG_GAME.Core.Entities.Battles;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class BattleStateAlreadyExistsException : DomainException
    {
        public BattleStatus BattleStatus { get; }

        public BattleStateAlreadyExistsException(BattleStatus battleStatus) : base($"BattleState with status: '{battleStatus}' already exists")
        {
            BattleStatus = battleStatus;
        }
    }
}
