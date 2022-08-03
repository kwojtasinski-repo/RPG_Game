namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidPlayerIdException : DomainException
    {
        public InvalidPlayerIdException() : base("Invalid PlayerId")
        {
        }
    }
}
