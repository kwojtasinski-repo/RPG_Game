namespace RPG_GAME.Application.Exceptions.Players
{
    public sealed class PlayerNotFoundException : BusinessException
    {
        public Guid PlayerId { get; }

        public PlayerNotFoundException(Guid playerId) : base($"Player with id '{playerId}' was not found.")
        {
            PlayerId = playerId;
        }
    }
}
