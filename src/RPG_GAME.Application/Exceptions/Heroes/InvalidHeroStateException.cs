namespace RPG_GAME.Application.Exceptions.Heroes
{
    public sealed class InvalidHeroStateException : BusinessException
    {
        public string Name { get; }

        public InvalidHeroStateException(string name) : base($"Invalid hero state '{name}'")
        {
            Name = name;
        }
    }
}
