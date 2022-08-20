namespace RPG_GAME.Application.Exceptions.Auth
{
    public sealed class EmailInUseException : BusinessException
    {
        public EmailInUseException() : base("Email is already in use.")
        {
        }
    }
}
