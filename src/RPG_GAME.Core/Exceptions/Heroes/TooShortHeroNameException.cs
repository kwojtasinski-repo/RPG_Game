namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class TooShortHeroNameException : DomainException
    {
        public string HeroName { get; }

        public TooShortHeroNameException(string heroName) : base($"HeroName: '{heroName}' is too short, expected at least 3 characters")
        {
            HeroName = heroName;
        }
    }
}
