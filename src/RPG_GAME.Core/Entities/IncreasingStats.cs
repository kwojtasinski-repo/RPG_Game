using System;

namespace RPG_GAME.Core.Entities
{
    public class IncreasingStats<T>
        where T : struct
    {
        public StrategyIncreasing StrategyIncreasing { get; set; }
        public T Value { get; set; }
    }
}
