using System;

namespace RPG_GAME.Core.Entities
{
    public class Skill : Skill<int>
    {
    }

    public class Skill<T>
        where T : struct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public decimal Probability { get; set; }
        public IncreasingStats<T> IncreasingStats { get; set; }
    }
}
