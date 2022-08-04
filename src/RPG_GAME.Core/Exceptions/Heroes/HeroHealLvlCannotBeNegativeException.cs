namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroHealLvlCannotBeNegativeException : DomainException
    {
        public int HealLvl { get; }

        public HeroHealLvlCannotBeNegativeException(int healLvl) : base($"Hero HealLvl: '{healLvl}' cannot be negative")
        {
            HealLvl = healLvl;
        }
    }
}
