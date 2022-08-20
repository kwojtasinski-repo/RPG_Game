namespace RPG_GAME.Application.Exceptions.Auth
{
    public sealed class UserNotFoundException : BusinessException
    {
        public Guid Id { get; }

        public UserNotFoundException(Guid id) : base($"User with id: '{id}' was not found")
        {
            Id = id;
        }
    }
}
