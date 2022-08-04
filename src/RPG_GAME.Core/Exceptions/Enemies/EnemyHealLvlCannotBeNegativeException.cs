namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class EnemyHealLvlCannotBeNegativeException : DomainException
    {
        public int HealLvl { get; }

        public EnemyHealLvlCannotBeNegativeException(int healLvl) : base($"Enemy HealLvl: '{healLvl}' cannot be negative")
        {
            HealLvl = healLvl;
        }
    }
}
