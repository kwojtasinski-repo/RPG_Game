namespace RPG_GAME.Application.Exceptions.Enemies
{
    internal sealed class InvalidEnemyIncreasingStateException : BusinessException
    {
        public InvalidEnemyIncreasingStateException() : base("Invalid enemy increasing state")
        {
        }
    }
}
