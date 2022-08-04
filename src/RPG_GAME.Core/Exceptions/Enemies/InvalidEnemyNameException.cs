namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidEnemyNameException : DomainException
    {
        public InvalidEnemyNameException() : base("Invalid EnemyName")
        {
        }
    }
}
