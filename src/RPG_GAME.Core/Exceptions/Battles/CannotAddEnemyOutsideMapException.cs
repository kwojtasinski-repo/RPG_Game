using System;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class CannotAddEnemyOutsideMapException : DomainException
    {
        public Guid EnemyId { get; }

        public CannotAddEnemyOutsideMapException(Guid enemyId) : base($"Cannot add Enemy with id: '{enemyId}' which is outside map")
        {

        }
    }
}
