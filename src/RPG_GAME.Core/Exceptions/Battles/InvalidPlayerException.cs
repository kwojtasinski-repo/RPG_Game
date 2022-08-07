namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class InvalidPlayerException : DomainException
    {
        public InvalidPlayerException() : base("Invalid Player")
        {
        }
    }
}
