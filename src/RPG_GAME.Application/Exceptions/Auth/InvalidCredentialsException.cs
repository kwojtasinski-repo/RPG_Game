namespace RPG_GAME.Application.Exceptions.Auth
{
    internal class InvalidCredentialsException : BusinessException
    {
        public InvalidCredentialsException() : base("Invalid credentials.")
        {
        }
    }
}
