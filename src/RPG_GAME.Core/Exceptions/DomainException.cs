using System;

namespace RPG_GAME.Core.Exceptions
{
    internal abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {
        }
    }
}
