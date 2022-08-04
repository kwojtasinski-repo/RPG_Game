namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroAttackCannotBeZeroOrNegativeException : DomainException
    {
        public int Attack { get; }

        public HeroAttackCannotBeZeroOrNegativeException(int attack) : base($"Hero Attack: '{attack}' cannot be zero or negative")
        {
            Attack = attack;
        }
    }
}
