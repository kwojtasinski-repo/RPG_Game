using System;

namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class MapAlreadyExistsException : DomainException
    {
        public Guid MapId { get; }

        public MapAlreadyExistsException(Guid mapId) : base($"Map with id '{mapId}' already exists")
        {
            MapId = mapId;
        }
    }
}
