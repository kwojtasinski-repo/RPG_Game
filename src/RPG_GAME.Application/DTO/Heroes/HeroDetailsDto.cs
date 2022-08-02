namespace RPG_GAME.Application.DTO.Heroes
{
    public class HeroDetailsDto
    {
        public Guid Id { get; set; }
        public string HeroName { get; set; }
        public FieldDto<int> Health { get; set; }
        public FieldDto<int> Attack { get; set; }
        public FieldDto<int> HealLvl { get; set; }
        public FieldDto<decimal> BaseRequiredExperience { get; set; }
        public IEnumerable<SkillDetailsHeroDto> Skills { get; set; }
        public IEnumerable<Guid> PlayersAssignedTo { get; set; }
    }
}
