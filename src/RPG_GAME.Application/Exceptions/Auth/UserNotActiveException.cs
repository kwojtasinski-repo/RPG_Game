namespace RPG_GAME.Application.Exceptions.Auth
{
    internal sealed class UserNotActiveException : BusinessException
    {
        public Guid UserId { get; }

        public UserNotActiveException(Guid userId) : base($"User with ID: '{userId}' is not active.")
        {
            UserId = userId;
        }
    }
}
