namespace RPG_GAME.Application.Exceptions.Maps
{
    internal sealed class MapNotFoundException : BusinessException
    {
        public Guid MapId { get; }

        public MapNotFoundException(Guid mapId) : base($"Map with id: '{mapId}' was not found")
        {
            MapId = mapId;
        }
    }
}
