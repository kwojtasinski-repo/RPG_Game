using System;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class EnemiesKilledQuantityHasBeenExceededException : DomainException
    {
        public Guid EnemyId { get; }

        public EnemiesKilledQuantityHasBeenExceededException(Guid enemyId) : base($"Enemies with id: '{enemyId}' killed quantity has been exceeded")
        {
            EnemyId = enemyId;
        }
    }
}
