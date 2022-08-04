namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyAttackCannotBeNegativeException : DomainException
    {
        public int Attack { get; }

        public EnemyAttackCannotBeNegativeException(int attack) : base($"Enemy Attack: '{attack}' cannot be negative")
        {
            Attack = attack;
        }
    }
}
