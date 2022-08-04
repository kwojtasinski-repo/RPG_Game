namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidMapIdException : DomainException
    {
        public InvalidMapIdException() : base("Invalid MapId")
        {
        }
    }
}
