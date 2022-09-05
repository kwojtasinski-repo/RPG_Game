namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class InvalidEnemyStateException : BusinessException
    {
        public string Name { get; }

        public InvalidEnemyStateException(string name) : base($"Invalid enemy state '{name}'")
        {
            Name = name;
        }
    }
}
