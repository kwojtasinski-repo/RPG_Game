namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyHealLvlCannotBeZeroOrNegativeException : DomainException
    {
        public int HealLvl { get; }

        public EnemyHealLvlCannotBeZeroOrNegativeException(int healLvl) : base($"Enemy HealLvl: '{healLvl}' cannot be zero or negative")
        {
            HealLvl = healLvl;
        }
    }
}
