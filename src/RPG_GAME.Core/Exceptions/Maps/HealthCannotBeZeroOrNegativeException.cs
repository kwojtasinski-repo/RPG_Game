namespace RPG_GAME.Core.Exceptions.Maps
{
    internal class HealthCannotBeZeroOrNegativeException : DomainException
    {
        public int Health { get; }

        public HealthCannotBeZeroOrNegativeException(int health) : base($"Health '{health}' cannot be zero or negative")
        {
            Health = health;
        }
    }
}
