using System;

namespace RPG_GAME.Core.NewEntities
{
    public class IncreasingStats
    {
        public Guid Id { get; set; }
        public Guid ObjectId { get; set; }
        public StrategyIncreasing StrategyIncreasing { get; set; }
        public string Field { get; set; }
        public int Value { get; set; }
    }
}
