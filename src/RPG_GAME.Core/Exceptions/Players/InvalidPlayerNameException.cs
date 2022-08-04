namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class InvalidPlayerNameException : DomainException
    {
        public InvalidPlayerNameException() : base("Invalid Player Name")
        {
        }
    }
}
