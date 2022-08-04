namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyHealthCannotBeNegativeException : DomainException
    {
        public int Health { get; }

        public EnemyHealthCannotBeNegativeException(int health) : base($"Enemy Health: '{health}' cannot be negative")
        {
            Health = health;
        }
    }
}
