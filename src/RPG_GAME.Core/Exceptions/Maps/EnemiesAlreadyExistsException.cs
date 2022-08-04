using System;

namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class EnemiesAlreadyExistsException : DomainException
    {
        public Guid EnemiesId { get; }

        public EnemiesAlreadyExistsException(Guid enemiesId) : base($"Enemies with id: '{enemiesId}' already exists")
        {
            EnemiesId = enemiesId;
        }
    }
}
