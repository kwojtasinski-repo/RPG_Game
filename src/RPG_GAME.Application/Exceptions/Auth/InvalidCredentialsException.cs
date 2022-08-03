namespace RPG_GAME.Application.Exceptions.Auth
{
    internal sealed class InvalidCredentialsException : BusinessException
    {
        public InvalidCredentialsException() : base("Invalid credentials.")
        {
        }
    }
}
