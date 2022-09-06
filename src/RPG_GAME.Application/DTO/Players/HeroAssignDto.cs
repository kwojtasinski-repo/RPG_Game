namespace RPG_GAME.Application.DTO.Players
{
    public class HeroAssignDto
    {
        public Guid Id { get; set; }
        public string HeroName { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public IEnumerable<SkillHeroAssignDto> Skills { get; set; }
    }
}
