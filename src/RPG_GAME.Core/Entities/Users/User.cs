﻿using RPG_GAME.Core.ValueObjects;
using System;

namespace RPG_GAME.Core.Entities.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public Email Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
