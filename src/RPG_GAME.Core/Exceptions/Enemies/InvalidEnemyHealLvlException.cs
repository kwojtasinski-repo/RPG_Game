namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidEnemyHealLvlException : DomainException
    {
        public InvalidEnemyHealLvlException() : base("Invalid Enemy HealLvl")
        {
        }
    }
}
