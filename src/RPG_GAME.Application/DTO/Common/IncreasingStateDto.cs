namespace RPG_GAME.Application.DTO.Common
{
    public class IncreasingStateDto<T>
        where T : struct
    {
        public string StrategyIncreasing { get; set; }
        public T Value { get; set; }
    }
}
