namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidEnemyAttackException : DomainException
    {
        public InvalidEnemyAttackException() : base("Invalid Enemy Attack")
        {
        }
    }
}
