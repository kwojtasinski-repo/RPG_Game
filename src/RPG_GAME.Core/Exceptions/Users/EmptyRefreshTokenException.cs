using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Exceptions.Users
{
    internal sealed class EmptyRefreshTokenException : DomainException
    {
        public EmptyRefreshTokenException() : base("Empty refresh token.")
        {
        }
    }
}
