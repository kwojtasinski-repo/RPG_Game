namespace RPG_GAME.Application.Exceptions.Heroes
{
    internal sealed class InvalidHeroStrategyIncreasingException : BusinessException
    {
        public string StrategyIncreasing { get; }

        public InvalidHeroStrategyIncreasingException(string strategyIncreasing) : base($"Invalid hero strategy increasing '{strategyIncreasing}'")
        {
            StrategyIncreasing = strategyIncreasing;
        }
    }
}
