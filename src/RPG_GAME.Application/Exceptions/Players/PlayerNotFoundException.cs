namespace RPG_GAME.Application.Exceptions.Players
{
    internal sealed class PlayerNotFoundException : BusinessException
    {
        public Guid Id { get; }

        public PlayerNotFoundException(Guid id) : base($"Player with id '{id}' was not found.")
        {
            Id = id;
        }
    }
}
