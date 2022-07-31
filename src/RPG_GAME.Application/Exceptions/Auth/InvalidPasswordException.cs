namespace RPG_GAME.Application.Exceptions.Auth
{
    internal class InvalidPasswordException : BusinessException
    {
        public InvalidPasswordException() : base("Password should have at least eight characters, one uppercase letter, one lowercase letter and one number")
        {
        }
    }
}
