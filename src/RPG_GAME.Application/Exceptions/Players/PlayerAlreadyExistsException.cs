namespace RPG_GAME.Application.Exceptions.Players
{
    public sealed class PlayerAlreadyExistsException : BusinessException
    {
        public Guid UserId { get; }

        public PlayerAlreadyExistsException(Guid userId) : base($"Player for User with id '{userId}' already exists.")
        {
            UserId = userId;
        }
    }
}
