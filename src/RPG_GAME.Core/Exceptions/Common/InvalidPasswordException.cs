using System;

namespace RPG_GAME.Core.Exceptions.Common
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base($"Invalid password. Password should have at least 8 characters, including upper letter and number")
        {
        }
    }
}
