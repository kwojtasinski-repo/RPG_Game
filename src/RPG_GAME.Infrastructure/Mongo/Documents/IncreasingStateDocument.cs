namespace RPG_GAME.Infrastructure.Mongo.Documents
{
    internal class IncreasingStateDocument<T>
        where T : struct
    {
        public StrategyIncreasing StrategyIncreasing { get; set; }
        public T Value { get; set; }
    }
}
