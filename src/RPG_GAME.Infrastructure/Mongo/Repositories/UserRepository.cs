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

        public async Task<User> GetAsync(Guid id)
        {
            var user = await _repository.GetAsync(x => x.Id == id);
            return user?.AsEntity();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var exists = await _repository.ExistsAsync(x => x.Id == id);
            return exists;
        }

        public async Task UpdateAsync(User user)
        {
            await _repository.UpdateAsync(user.AsDocument());
        }
    }
}
