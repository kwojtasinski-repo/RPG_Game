using System;

namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidPlayerIdException : DomainException
    {
        public Guid PlayerId { get; }

        public InvalidPlayerIdException(Guid playerId) : base($"Invalid Player identifier: '{playerId}'")
            => PlayerId = playerId;

        public InvalidPlayerIdException(string playerId) : base($"Invalid Player identifier: '{playerId}'")
            => PlayerId = Guid.Empty;
    }
}
