namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyHealthCannotBeZeroOrNegativeException : DomainException
    {
        public int Health { get; }

        public EnemyHealthCannotBeZeroOrNegativeException(int health) : base($"Enemy Health: '{health}' cannot be zero or negative")
        {
            Health = health;
        }
    }
}
