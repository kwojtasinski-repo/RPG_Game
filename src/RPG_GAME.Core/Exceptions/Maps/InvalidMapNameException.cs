namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class InvalidMapNameException : DomainException
    {
        public InvalidMapNameException() : base("Invalid Map Name")
        {
        }
    }
}
