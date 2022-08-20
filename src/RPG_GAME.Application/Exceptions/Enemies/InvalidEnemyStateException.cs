namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class InvalidEnemyStateException : BusinessException
    {
        public InvalidEnemyStateException() : base("Invalid enemy state")
        {
        }
    }
}
