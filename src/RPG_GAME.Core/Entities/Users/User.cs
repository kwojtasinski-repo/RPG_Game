using RPG_GAME.Core.Exceptions.Common;
using RPG_GAME.Core.Exceptions.Players;
using RPG_GAME.Core.ValueObjects;
using System;

namespace RPG_GAME.Core.Entities.Users
{
    public class User
    {
        public Guid Id { get; }
        public Email Email { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }
        public DateTime CreatedAt { get; }
        public bool IsActive { get; private set; }

        public User(Guid id, string email, string password, string role, DateTime createdAt, bool isActive = true)
        {
            Id = id;
            ChangeEmail(email);
            ChangePassword(password);
            ChangeRole(role);
            CreatedAt = createdAt;
            IsActive = isActive;
        }

        public static User Create(string email, string password, DateTime createdAt, string role)
        {
            return new User(Guid.NewGuid(), email, password, role, createdAt);
        }

        public void ChangeEmail(string email)
        {
            Email = email;
        }

        public void ChangePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidPasswordException();
            }

            Password = password;
        }

        public void ChangeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                throw new InvalidRoleException();
            }

            Role = role;
        }

        public void ChangeUserActivity(bool isActive)
        {
            IsActive = isActive;
        }
    }
}
