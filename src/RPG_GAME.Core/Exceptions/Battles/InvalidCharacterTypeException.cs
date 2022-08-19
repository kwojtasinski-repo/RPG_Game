namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class InvalidCharacterTypeException : DomainException
    {
        public string Character { get; }

        public InvalidCharacterTypeException(string character) : base($"Invalid character '{character}'")
        {
            Character = character;
        }
    }
}
