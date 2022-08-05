namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class InvalidMapDifficultyException : DomainException
    {
        public string Difficulty { get; }

        public InvalidMapDifficultyException(string difficulty) : base($"Invalid Map Difficulty '{difficulty}'")
        {
            Difficulty = difficulty;
        }
    }
}
