namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroHealthCannotBeZeroOrNegativeException : DomainException
    {
        public int Health { get; }

        public HeroHealthCannotBeZeroOrNegativeException(int health) : base($"Hero Health: '{health}' cannot be zero or negative")
        {
            Health = health;
        }
    }
}
