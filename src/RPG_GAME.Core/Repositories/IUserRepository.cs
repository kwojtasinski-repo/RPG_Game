using RPG_GAME.Core.Entities.Users;
using System;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetAsync(string email);
        public Task<User> GetAsync(Guid id);
        public Task<bool> ExistsAsync(Guid id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
