namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class InvalidEnemyException : DomainException
    {
        public InvalidEnemyException() : base("Invalid Enemy")
        {
        }
    }
}
