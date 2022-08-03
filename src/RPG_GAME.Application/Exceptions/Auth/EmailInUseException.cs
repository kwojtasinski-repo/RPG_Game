namespace RPG_GAME.Application.Exceptions.Auth
{
    internal sealed class EmailInUseException : BusinessException
    {
        public EmailInUseException() : base("Email is already in use.")
        {
        }
    }
}
