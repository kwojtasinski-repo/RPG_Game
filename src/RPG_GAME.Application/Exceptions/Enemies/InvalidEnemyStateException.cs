namespace RPG_GAME.Application.Exceptions.Enemies
{
    internal sealed class InvalidEnemyStateException : BusinessException
    {
        public InvalidEnemyStateException() : base("Invalid enemy state")
        {
        }
    }
}
