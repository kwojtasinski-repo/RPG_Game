namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidIncreasingStateException : DomainException
    {
        public InvalidIncreasingStateException() : base("Invalid IncreasingState")
        {
        }
    }
}
