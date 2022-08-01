using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_GAME.Core.Entities
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
        public IncreasingStats<T> IncreasingStats { get; set; }
    }
}
