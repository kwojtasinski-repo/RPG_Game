namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroAttackCannotBeNegativeException : DomainException
    {
        public int Attack { get; }

        public HeroAttackCannotBeNegativeException(int attack) : base($"Hero Attack: '{attack}' cannot be negative")
        {
            Attack = attack;
        }
    }
}
