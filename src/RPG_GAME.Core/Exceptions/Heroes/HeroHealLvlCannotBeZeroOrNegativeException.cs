namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroHealLvlCannotBeZeroOrNegativeException : DomainException
    {
        public int HealLvl { get; }

        public HeroHealLvlCannotBeZeroOrNegativeException(int healLvl) : base($"Hero HealLvl: '{healLvl}' cannot be zero or negative")
        {
            HealLvl = healLvl;
        }
    }
}
