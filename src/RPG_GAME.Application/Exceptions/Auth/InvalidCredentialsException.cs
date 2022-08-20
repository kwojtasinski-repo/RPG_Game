namespace RPG_GAME.Application.Exceptions.Auth
{
    public sealed class InvalidCredentialsException : BusinessException
    {
        public InvalidCredentialsException() : base("Invalid credentials.")
        {
        }
    }
}
