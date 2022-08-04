namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyAttackCannotBeZeroOrNegativeException : DomainException
    {
        public int Attack { get; }

        public EnemyAttackCannotBeZeroOrNegativeException(int attack) : base($"Enemy Attack: '{attack}' cannot be zero or negative")
        {
            Attack = attack;
        }
    }
}
