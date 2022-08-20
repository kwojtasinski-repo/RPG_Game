using System;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class BattleStatesNotFoundException : DomainException
    {
        public Guid BattleId { get; }

        public BattleStatesNotFoundException(Guid battleId) : base($"BattleStates for Battle with id: '{battleId}' was not found")
        {
            BattleId = battleId;
        }
    }
}
