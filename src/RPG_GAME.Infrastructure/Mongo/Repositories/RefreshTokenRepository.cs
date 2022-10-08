using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Infrastructure.Mongo.Documents.Users;
using RPG_GAME.Infrastructure.Mongo.Mappings;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoRepository<RefreshTokenDocument, Guid> _repository;

        public RefreshTokenRepository(IMongoRepository<RefreshTokenDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(RefreshToken refreshToken)
        {
            return _repository.AddAsync(refreshToken.AsDocument());
        }

        public async Task<RefreshToken> GetAsync(string token)
        {
            var refreshToken = await _repository.GetAsync(x => x.Token == token);
            return refreshToken?.AsEntity();
        }

        public Task UpdateAsync(RefreshToken refreshToken)
        {
            return _repository.UpdateAsync(refreshToken.AsDocument());
        }
    }
}
