using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Application.Mappings;

namespace RPG_GAME.Application.Events.Heroes
{
    internal class HeroUpdated : IEvent
    {
        public Guid HeroId { get; }
        public string HeroName { get; }
        public IEnumerable<SkillHero> Skills { get; } = new List<SkillHero>();
        public IEnumerable<SkillHeroAssign> SkillsToUpdate { get; } = new List<SkillHeroAssign>();

        public HeroUpdated(Guid heroId, string heroName, IEnumerable<SkillHero> skills = null)
        {
            HeroId = heroId;
            HeroName = heroName;

            if (skills is not null)
            {
                Skills = skills;
                SkillsToUpdate = skills.Select(s => s.AsAssign());
            }
        }

    }
}
