using System;

namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidBattleIdException : DomainException
    {
        public Guid BattleId { get; }

        public InvalidBattleIdException(Guid battleId) : base($"Invalid Battle identifier: '{battleId}'")
            => BattleId = battleId;

        public InvalidBattleIdException(string battleId) : base($"Invalid Battle identifier: '{battleId}'")
            => BattleId = Guid.Empty;
    }
}
