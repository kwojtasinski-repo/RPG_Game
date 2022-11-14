using RPG_GAME.Core.Entities.Battles.Actions;
using System;

namespace RPG_GAME.Core.Entities.Battles
{
    public class BattleEvent
    {
        public Guid Id { get; }
        public Guid BattleId { get; }
        public FightAction Action { get; }
        public int Level { get; }
        public decimal CurrentExp { get; }
        public decimal RequiredExp { get; }
        public DateTime Created { get; }

        public BattleEvent(Guid id, Guid battleId, FightAction action, int level, decimal currentExp, decimal requiredExp, DateTime created)
        {
            Id = id;
            BattleId = battleId;
            Action = action;
            Level = level;
            CurrentExp = currentExp;
            RequiredExp = requiredExp;
            Created = created;
        }
    }
}
