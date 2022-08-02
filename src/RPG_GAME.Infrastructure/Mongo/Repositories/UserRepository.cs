using RPG_GAME.Infrastructure.Mongo.Mappings;
using RPG_GAME.Infrastructure.Mongo.Documents;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Core.Entities.Users;

namespace RPG_GAME.Infrastructure.Mongo.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<UserDocument, Guid> _repository;

        public UserRepository(IMongoRepository<UserDocument, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(User user)
        {
            return _repository.AddAsync(user.AsDocument());
        }

        public async Task<User> GetAsync(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());
            return user?.AsEntity();
        }
    }
}
