namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class InvalidHeroNameException : DomainException
    {
        public InvalidHeroNameException() : base($"Invalid HeroName")
        {
        }
    }
}
