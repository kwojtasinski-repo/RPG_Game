namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class TooShortEnemyNameException : DomainException
    {
        public string EnemyName { get; }

        public TooShortEnemyNameException(string enemyName) : base($"EnemyName: '{enemyName}' is too short, expected at least 3 characters")
        {
            EnemyName = enemyName;
        }
    }
}
