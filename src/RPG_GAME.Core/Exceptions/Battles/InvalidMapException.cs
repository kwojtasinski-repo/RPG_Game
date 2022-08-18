using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal class InvalidMapException : DomainException
    {
        public InvalidMapException() : base("Invalid Map")
        {
        }
    }
}
