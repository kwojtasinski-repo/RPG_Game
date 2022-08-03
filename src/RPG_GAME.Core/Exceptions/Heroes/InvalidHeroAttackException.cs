namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidHeroAttackException : DomainException
    {
        public InvalidHeroAttackException() : base("Invalid hero Attack")
        {
        }
    }
}
