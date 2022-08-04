namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class PlayerLevelCannotZeroOrNegativeException : DomainException
    {
        public int Level { get; }

        public PlayerLevelCannotZeroOrNegativeException(int level) : base($"Player level: '{level}' cannot be zero or negative")
        {
            Level = level;
        }
    }
}
