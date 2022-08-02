using System;

namespace RPG_GAME.Core.Entities.Common
{
    public class IncreasingState<T>
        where T : struct
    {
        public StrategyIncreasing StrategyIncreasing { get; set; }
        public T Value { get; set; }
    }
}
