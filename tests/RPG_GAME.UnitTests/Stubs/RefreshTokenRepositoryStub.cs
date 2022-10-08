using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class RefreshTokenRepositoryStub : IRefreshTokenRepository
    {
        private readonly List<RefreshToken> _refreshTokens = new();

        public Task AddAsync(RefreshToken refreshToken)
        {
            _refreshTokens.Add(refreshToken);
            return Task.CompletedTask;
        }

        public Task<RefreshToken> GetAsync(string token)
        {
            return Task.FromResult(_refreshTokens.Where(t => t.Token == token).SingleOrDefault());
        }

        public Task UpdateAsync(RefreshToken refreshToken)
        {
            return Task.CompletedTask;
        }
    }
}
