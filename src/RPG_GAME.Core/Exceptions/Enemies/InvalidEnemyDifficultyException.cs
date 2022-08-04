namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidEnemyDifficultyException : DomainException
    {
        public string Difficulty { get; }

        public InvalidEnemyDifficultyException(string difficulty) : base($"Invalid difficulty: '{difficulty}'")
        {
            Difficulty = difficulty;
        }
    }
}
