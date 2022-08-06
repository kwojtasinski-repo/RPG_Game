namespace RPG_GAME.Application.Events.Maps
{
    internal class EnemyRemovedFromMap : IEvent
    {
        public Guid EnemyId { get; }
        public Guid MapId { get; }
        public string MapName { get; }

        public EnemyRemovedFromMap(Guid enemyId, Guid mapId, string mapName)
        {
            EnemyId = enemyId;
            MapId = mapId;
            MapName = mapName;
        }
    }
}
