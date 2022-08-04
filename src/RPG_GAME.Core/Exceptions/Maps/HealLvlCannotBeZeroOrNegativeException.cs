namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class HealLvlCannotBeZeroOrNegativeException : DomainException
    {
        public int HealLvl { get; }

        public HealLvlCannotBeZeroOrNegativeException(int healLvl) : base($"HealLvl '{healLvl}' cannot be zero or negative")
        {
            HealLvl = healLvl;
        }
    }
}
