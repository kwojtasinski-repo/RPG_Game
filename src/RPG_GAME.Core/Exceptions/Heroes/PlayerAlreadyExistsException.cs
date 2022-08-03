using System;

namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class PlayerAlreadyExistsException : DomainException
    {
        public Guid PlayerId { get; }

        public PlayerAlreadyExistsException(Guid playerId) : base($"Player with id '{playerId}' already exists")
        {
            PlayerId = playerId;
        }
    }
}
