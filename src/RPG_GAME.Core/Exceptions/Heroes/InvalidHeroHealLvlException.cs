namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidHeroHealLvlException : DomainException
    {
        public InvalidHeroHealLvlException() : base("Invalid hero HealLvl")
        {
        }
    }
}
