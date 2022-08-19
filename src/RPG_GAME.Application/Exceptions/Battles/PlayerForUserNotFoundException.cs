namespace RPG_GAME.Application.Exceptions.Battles
{
    internal sealed class PlayerForUserNotFoundException : BusinessException
    {
        public Guid UserId { get; }

        public PlayerForUserNotFoundException(Guid userId) : base($"Player for user with id: '{userId}' was not found")
        {
        }
    }
}
