using System;

namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidMapIdException : DomainException
    {
        public Guid MapId { get; }

        public InvalidMapIdException(Guid mapId) : base($"Invalid Map identifier: '{mapId}'")
            => MapId = mapId;

        public InvalidMapIdException(string mapId) : base($"Invalid Map identifier: '{mapId}'")
            => MapId = Guid.Empty;
    }
}
