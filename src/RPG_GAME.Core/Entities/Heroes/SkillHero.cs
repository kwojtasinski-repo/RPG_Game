using System;
using RPG_GAME.Core.Entities.Common;

namespace RPG_GAME.Core.Entities.Heroes
{
    public class SkillHero : SkillHero<int>
    {
    }

    public class SkillHero<T>
        where T : struct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public IncreasingState<T> IncreasingState { get; set; }
    }
}
