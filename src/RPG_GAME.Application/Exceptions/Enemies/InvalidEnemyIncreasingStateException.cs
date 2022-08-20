namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class InvalidEnemyIncreasingStateException : BusinessException
    {
        public InvalidEnemyIncreasingStateException() : base("Invalid enemy increasing state")
        {
        }
    }
}
