namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroHealthCannotBeNegativeException : DomainException
    {
        public int Health { get; }

        public HeroHealthCannotBeNegativeException(int health) : base($"Hero Health: '{health}' cannot be negative")
        {
            Health = health;
        }
    }
}
