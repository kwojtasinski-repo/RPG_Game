namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class InvalidEnemyDifficultyException : BusinessException
    {
        public string Difficulty { get; }

        public InvalidEnemyDifficultyException(string difficulty) : base($"Invalid enemy difficulty '{difficulty}'")
        {
            Difficulty = difficulty;
        }
    }
}
