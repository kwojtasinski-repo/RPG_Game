using RPG_GAME.Application.DTO.Common;

namespace RPG_GAME.Application.DTO.Heroes
{
    public class HeroDto
    {
        public Guid Id { get; set; }
        public string HeroName { get; set; }
        public StateDto<int> Health { get; set; }
        public StateDto<int> Attack { get; set; }
        public StateDto<int> HealLvl { get; set; }
        public StateDto<decimal> BaseRequiredExperience { get; set; }
        public IEnumerable<SkillHeroDto> Skills { get; set; }
    }
}
