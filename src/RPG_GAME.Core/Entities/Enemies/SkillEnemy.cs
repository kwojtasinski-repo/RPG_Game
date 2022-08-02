using System;
using RPG_GAME.Core.Entities.Common;

namespace RPG_GAME.Core.Entities.Enemies
{
    public class SkillEnemy : SkillEnemy<int>
    {
    }

    public class SkillEnemy<T>
        where T : struct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public decimal Probability { get; set; }
        public IncreasingState<T> IncreasingStats { get; set; }
    }
}
