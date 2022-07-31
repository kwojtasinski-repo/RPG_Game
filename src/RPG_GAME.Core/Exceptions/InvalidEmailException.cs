using System;

namespace RPG_GAME.Core.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public string Email { get; }

        public InvalidEmailException(string email) : base($"Invalid email: {email}.")
        {
            Email = email;
        }
    }
}
