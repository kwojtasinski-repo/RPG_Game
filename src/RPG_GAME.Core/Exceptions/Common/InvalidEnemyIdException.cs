using System;

namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidEnemyIdException : DomainException
    {
        public Guid EnemyId { get; }

        public InvalidEnemyIdException(Guid enemyId) : base($"Invalid Enemy identifier: '{enemyId}'")
            => EnemyId = enemyId;

        public InvalidEnemyIdException(string enemyId) : base($"Invalid Enemy identifier: '{enemyId}'")
            => EnemyId = Guid.Empty;
    }
}
