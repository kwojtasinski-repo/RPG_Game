namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class HeroAssignAttackCannotBeZeroOrNegativeException : DomainException
    {
        public int Attack { get; }

        public HeroAssignAttackCannotBeZeroOrNegativeException(int attack) : base($"Attack: '{attack}' cannot be zero or negative")
        {
            Attack = attack;
        }
    }
}
