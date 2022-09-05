namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class InvalidEnemyIncreasingStateException : BusinessException
    {
        public string Name { get; }

        public InvalidEnemyIncreasingStateException(string name) : base($"Invalid enemy increasing state '{name}'")
        {
            Name = name;
        }
    }
}
