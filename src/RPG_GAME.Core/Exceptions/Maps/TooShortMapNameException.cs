namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class TooShortMapNameException : DomainException
    {
        public string Name { get; }

        public TooShortMapNameException(string name) : base($"Map Name: '{name}' is too short, expecte at least 3 characters")
        {
            Name = name;
        }
    }
}
