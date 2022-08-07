using System;

namespace RPG_GAME.Core.Entities.Battles
{
    public class BattleEvent<T>
        where T : class, IAction
    {
        public Guid Id { get; }
        public Guid BattleId { get; }
        public T Action { get; }
        public DateTime Created { get; }

        public BattleEvent(Guid id, Guid battleId, T action, DateTime created)
        {
            Id = id;
            BattleId = battleId;
            Action = action;
            Created = created;
        }
    }
}
