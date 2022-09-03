namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class CannotPrepareBattleForMapWithEmptyEnemiesException : BusinessException
    {
        public Guid MapId { get; }

        public CannotPrepareBattleForMapWithEmptyEnemiesException(Guid mapId) : base($"Cannot prepare battle for map with id: '{mapId}', which not contains any enemies")
        {
            MapId = mapId;
        }
    }
}
