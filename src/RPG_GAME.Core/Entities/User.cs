using RPG_GAME.Core.ValueObjects;
using System;

namespace RPG_GAME.Core.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public Email Email { get; set; }
        public string Password { get; set; }
    }
}
