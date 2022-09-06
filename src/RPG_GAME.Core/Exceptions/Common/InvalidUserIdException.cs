using System;

namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidUserIdException : DomainException
    {
        public Guid UserId { get; }

        public InvalidUserIdException(Guid userId) : base($"Invalid User identifier: '{userId}'")
            => UserId = userId;

        public InvalidUserIdException(string userId) : base($"Invalid User identifier: '{userId}'")
            => UserId = Guid.Empty;
    }
}
