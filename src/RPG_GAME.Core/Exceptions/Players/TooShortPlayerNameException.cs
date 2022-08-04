namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class TooShortPlayerNameException : DomainException
    {
        public string Name { get; }

        public TooShortPlayerNameException(string name) : base($"Player Name: '{name}' is too short, expected at least 3 characters")
        {
            Name = name;
        }
    }
}
