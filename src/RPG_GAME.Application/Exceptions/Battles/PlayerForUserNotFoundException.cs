namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class PlayerForUserNotFoundException : BusinessException
    {
        public Guid UserId { get; }

        public PlayerForUserNotFoundException(Guid userId) : base($"Player for user with id: '{userId}' was not found")
        {
        }
    }
}
