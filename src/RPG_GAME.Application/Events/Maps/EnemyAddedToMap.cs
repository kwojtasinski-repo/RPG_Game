namespace RPG_GAME.Application.Events.Maps
{
    internal class EnemyAddedToMap : IEvent
    {
        public Guid EnemyId { get; }
        public Guid MapId { get; }
        public string MapName { get; }

        public EnemyAddedToMap(Guid enemyId, Guid mapId, string mapName)
        {
            EnemyId = enemyId;
            MapId = mapId;
            MapName = mapName;
        }
    }
}
