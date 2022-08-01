using RPG_GAME.Core.Entities;

namespace RPG_GAME.Application.DTO
{
    public class IncreasingStatsDto<T>
        where T : struct
    {
        public StrategyIncreasing StrategyIncreasing { get; set; }
        public T Value { get; set; }
    }
}
