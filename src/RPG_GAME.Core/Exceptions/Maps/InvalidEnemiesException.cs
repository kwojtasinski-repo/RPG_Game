namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class InvalidEnemiesException : DomainException
    {
        public InvalidEnemiesException() : base("Invalid Enemies")
        {
        }
    }
}
