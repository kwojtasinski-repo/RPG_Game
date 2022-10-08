namespace RPG_GAME.Application.Exceptions.Auth
{
    public sealed class RevokedRefreshTokenException : BusinessException
    {
        public RevokedRefreshTokenException() : base("Revoked refresh token.")
        {
        }
    }
}
