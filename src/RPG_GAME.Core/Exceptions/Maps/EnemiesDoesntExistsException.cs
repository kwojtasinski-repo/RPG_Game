using System;

namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class EnemiesDoesntExistsException : DomainException
    {
        public Guid EnemiesId { get; }

        public EnemiesDoesntExistsException(Guid enemiesId) : base($"Enemies with id: '{enemiesId}' doesnt exists")
        {
            EnemiesId = enemiesId;
        }
    }
}
