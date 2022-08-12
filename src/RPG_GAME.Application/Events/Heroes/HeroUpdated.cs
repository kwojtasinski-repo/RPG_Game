using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Events.Heroes
{
    internal class HeroUpdated : IEvent
    {
        public Guid HeroId { get; }
        public string HeroName { get; }
        public IEnumerable<SkillHeroAssign> Skills { get; } = new List<SkillHeroAssign>();

        public HeroUpdated(Guid heroId, string heroName, IEnumerable<SkillHeroAssign> skills = null)
        {
            HeroId = heroId;
            HeroName = heroName;

            if (skills is not null)
            {
                Skills = skills;
            }
        }

    }
}
