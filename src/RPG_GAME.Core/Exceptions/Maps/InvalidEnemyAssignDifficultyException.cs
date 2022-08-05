namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class InvalidEnemyAssignDifficultyException : DomainException
    {
        public string Difficulty { get; }

        public InvalidEnemyAssignDifficultyException(string difficulty) : base($"Invalid EnemyAssign Difficulty '{difficulty}'")
        {
            Difficulty = difficulty;
        }
    }
}
