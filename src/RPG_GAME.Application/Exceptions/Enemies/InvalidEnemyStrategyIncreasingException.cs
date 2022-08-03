namespace RPG_GAME.Application.Exceptions.Enemies
{
    internal sealed class InvalidEnemyStrategyIncreasingException : BusinessException
    {
        public string StrategyIncreasing { get; }

        public InvalidEnemyStrategyIncreasingException(string strategyIncreasing) : base($"Invalid enemy strategy increasing '{strategyIncreasing}'")
        {
            StrategyIncreasing = strategyIncreasing;
        }
    }
}
