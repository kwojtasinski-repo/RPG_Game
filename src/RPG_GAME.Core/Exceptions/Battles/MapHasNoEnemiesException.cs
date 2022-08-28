using System;

namespace RPG_GAME.Core.Exceptions.Battles
{
    public sealed class MapHasNoEnemiesException : DomainException
    {
        public Guid MapId { get; }

        public MapHasNoEnemiesException(Guid mapId) : base($"Map with id: '{mapId}' has no enemies")
        {
            MapId = mapId;
        }
    }
}
