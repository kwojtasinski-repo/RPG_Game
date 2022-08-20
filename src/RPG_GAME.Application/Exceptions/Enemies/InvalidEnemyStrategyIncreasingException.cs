namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class InvalidEnemyStrategyIncreasingException : BusinessException
    {
        public string StrategyIncreasing { get; }

        public InvalidEnemyStrategyIncreasingException(string strategyIncreasing) : base($"Invalid enemy strategy increasing '{strategyIncreasing}'")
        {
            StrategyIncreasing = strategyIncreasing;
        }
    }
}
