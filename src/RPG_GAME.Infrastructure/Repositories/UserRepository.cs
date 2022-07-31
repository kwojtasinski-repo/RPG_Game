using RPG_GAME.Core.Entities;
using RPG_GAME.Core.Repositories;

namespace RPG_Game.Infrastructure.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<User, Guid> _repository;

        public UserRepository(IMongoRepository<User, Guid> repository)
        {
            _repository = repository;
        }

        public Task AddAsync(User user)
        {
            return _repository.AddAsync(user);
        }

        public async Task<User> GetAsync(string email)
        {
            var user = await _repository.GetAsync(x => x.Email == email.ToLowerInvariant());
            return user;
        }
    }
}
