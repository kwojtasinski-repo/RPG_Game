using RPG_GAME.Core.Entities.Battles.Actions;
using System;

namespace RPG_GAME.Core.Entities.Battles
{
    public class BattleEvent
    {
        public Guid Id { get; }
        public Guid BattleId { get; }
        public FightAction Action { get; }
        public DateTime Created { get; }

        public BattleEvent(Guid id, Guid battleId, FightAction action, DateTime created)
        {
            Id = id;
            BattleId = battleId;
            Action = action;
            Created = created;
        }
    }
}
