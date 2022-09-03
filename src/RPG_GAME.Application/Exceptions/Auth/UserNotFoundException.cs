﻿namespace RPG_GAME.Application.Exceptions.Auth
{
    public sealed class UserNotFoundException : BusinessException
    {
        public Guid UserId { get; }

        public UserNotFoundException(Guid userId) : base($"User with id: '{userId}' was not found")
        {
            UserId = userId;
        }
    }
}
