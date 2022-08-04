namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidIncreasingStateValueException : DomainException
    {
        public InvalidIncreasingStateValueException() : base("Invalid IncreasingState Value")
        {
        }
    }
}
