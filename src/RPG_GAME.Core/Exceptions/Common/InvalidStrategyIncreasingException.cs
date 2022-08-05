namespace RPG_GAME.Core.Exceptions.Common
{
    internal sealed class InvalidStrategyIncreasingException : DomainException
    {
        public string StrategyIncreasing { get; }

        public InvalidStrategyIncreasingException(string strategyIncreasing) : base($"Invalid StrategyIncrasing '{strategyIncreasing}'")
        {
            StrategyIncreasing = strategyIncreasing;
        }
    }
}
