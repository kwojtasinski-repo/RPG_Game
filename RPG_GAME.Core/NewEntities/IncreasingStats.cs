using System;

namespace RPG_GAME.Core.NewEntities
{
    public class IncreasingStats
    {
        public Guid Id { get; set; }
        public Guid ObjectId { get; set; }
        public StrategyIncreasing StrategyIncreasing { get; set; }
        public int Value { get; set; }
        // TODO: How to assign this values to fields for example Health, Attack?
    }
}
