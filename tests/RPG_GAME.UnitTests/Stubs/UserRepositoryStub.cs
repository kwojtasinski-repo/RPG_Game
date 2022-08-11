using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPG_GAME.UnitTests.Stubs
{
    internal sealed class UserRepositoryStub : IUserRepository
    {
        private IList<User> _users = new List<User>();

        public Task AddAsync(User user)
        {
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(_users.SingleOrDefault(u => u.Id == id) is not null);
        }

        public Task<User> GetAsync(string email)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
        }

        public Task<User> GetAsync(Guid id)
        {
            return Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
        }

        public Task UpdateAsync(User user)
        {
            return Task.CompletedTask;
        }
    }
}
