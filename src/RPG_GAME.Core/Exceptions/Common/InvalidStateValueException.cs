namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidStateValueException : DomainException
    {
        public InvalidStateValueException() : base("Invalid StateValue")
        {
        }
    }
}
