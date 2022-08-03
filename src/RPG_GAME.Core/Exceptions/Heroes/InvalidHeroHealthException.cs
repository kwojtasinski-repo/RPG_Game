namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidHeroHealthException : DomainException
    {
        public InvalidHeroHealthException() : base("Invalid hero Health")
        {
        }
    }
}
