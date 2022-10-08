namespace RPG_GAME.Application.Exceptions.Auth
{
    public sealed class InvalidRefreshTokenException : BusinessException
    {
        public InvalidRefreshTokenException() : base("Invalid refresh token.")
        {
        }
    }
}
