using RPG_GAME.Application.DTO.Auth;

namespace RPG_GAME.Application.Auth
{
    public interface IAuthManager
    {
        JsonWebToken CreateToken(string userId, string userEmail, string role = null, string audience = null,
                  IDictionary<string, IEnumerable<string>> claims = null);
    }
}
