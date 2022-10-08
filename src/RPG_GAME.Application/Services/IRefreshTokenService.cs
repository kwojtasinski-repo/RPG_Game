using RPG_GAME.Application.DTO.Auth;

namespace RPG_GAME.Application.Services
{
    public interface IRefreshTokenService
    {
        Task<string> CreateAsync(Guid userId);
        Task<JsonWebToken> UseAsync(string refreshToken);
    }
}
