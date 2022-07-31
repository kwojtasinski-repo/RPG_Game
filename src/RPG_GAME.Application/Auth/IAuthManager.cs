using RPG_GAME.Application.DTO;

namespace RPG_GAME.Application.Auth
{
    public interface IAuthManager
    {
        JsonWebToken CreateToken(string userId, string role = null, string audience = null,
                  IDictionary<string, IEnumerable<string>> claims = null);
    }
}
