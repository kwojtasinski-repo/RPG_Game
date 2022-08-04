using System;

namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class MapDoesntExistsException : DomainException
    {
        public Guid MapId { get; }

        public MapDoesntExistsException(Guid mapId) : base($"Map with id: '{mapId}' doesnt exists")
        {
            MapId = mapId;
        }
    }
}
