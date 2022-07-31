﻿using RPG_GAME.Core.Entities;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetAsync(string email);
        Task AddAsync(User user);
    }
}
