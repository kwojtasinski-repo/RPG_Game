using RPG_GAME.Core.Entities.Users;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetAsync(string token);
        Task AddAsync(RefreshToken refreshToken);
        Task UpdateAsync(RefreshToken refreshToken);
    }
}
