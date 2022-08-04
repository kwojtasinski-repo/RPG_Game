namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class PlayerSkillAttackCannotBeZeroOrNegativeException : DomainException
    {
        public int Attack { get; }

        public PlayerSkillAttackCannotBeZeroOrNegativeException(int attack) : base($"Player Skill Attack: '{attack}' cannot be zero or negative")
        {
            Attack = attack;
        }
    }
}
