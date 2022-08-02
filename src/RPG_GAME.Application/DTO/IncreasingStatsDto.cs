namespace RPG_GAME.Application.DTO
{
    public class IncreasingStatsDto<T>
        where T : struct
    {
        public string StrategyIncreasing { get; set; }
        public T Value { get; set; }
    }
}
