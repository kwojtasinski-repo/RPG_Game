namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class AttackCannotBeZeroOrNegativeException : DomainException
    {
        public int Attack { get; }

        public AttackCannotBeZeroOrNegativeException(int attack) : base($"Attack: '{attack}' cannot be zero or negative")
        {
            Attack = attack;
        }
    }
}
