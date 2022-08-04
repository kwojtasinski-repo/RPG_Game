namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class HeroAssignHealLvlCannotBeZeroOrNegativeException : DomainException
    {
        public int HealLvl { get; set; }

        public HeroAssignHealLvlCannotBeZeroOrNegativeException(int healLvl) : base($"HealLvl: '{healLvl}' cannot be zero or negative")
        {

        }
    }
}
