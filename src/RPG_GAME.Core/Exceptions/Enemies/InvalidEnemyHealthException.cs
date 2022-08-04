namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidEnemyHealthException : DomainException
    {
        public InvalidEnemyHealthException() : base("Invalid Enemy Health")
        {
        }
    }
}
