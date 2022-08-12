using RPG_GAME.Core.Entities.Players;
using System.Collections.Generic;

namespace RPG_GAME.Core.Services.Players
{
    public sealed class HeroAssignFieldsToUpdate
    {
        public string HeroName { get; }
        public IEnumerable<SkillHeroAssign> Skills { get; } = new List<SkillHeroAssign>();

        public HeroAssignFieldsToUpdate(string heroName, IEnumerable<SkillHeroAssign> skills)
        {
            HeroName = heroName;
            Skills = skills;
        }
    }
}
