namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class HeroAssignHealthCannotBeZeroOrNegativeException : DomainException
    {
        public int Health { get; }

        public HeroAssignHealthCannotBeZeroOrNegativeException(int health) : base($"Health: '{health}' cannot be zero or negative")
        {
            Health = health;
        }
    }
}
