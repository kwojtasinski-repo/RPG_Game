using System;

namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class PlayerDoesntExistsException : DomainException
    {
        public Guid PlayerId { get; }

        public PlayerDoesntExistsException(Guid playerId) : base($"Player with id: '{playerId}' doesnt exists")
        {
            PlayerId = playerId;
        }
    }
}
