namespace RPG_GAME.Application.Events.Players
{
    internal class PlayerAdded : IEvent
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }
        public Guid HeroId { get; }
        public Guid UserId { get; }

        public PlayerAdded(Guid playerId, string playerName, Guid heroId, Guid userId)
        {
            PlayerId = playerId;
            PlayerName = playerName;
            HeroId = heroId;
            UserId = userId;
        }
    }
}
