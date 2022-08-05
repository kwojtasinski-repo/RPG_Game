namespace RPG_GAME.Application.DTO.Players
{
    public class UpdatePlayerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal CurrentExp { get; set; }
        public decimal RequiredExp { get; set; }
        public int HeroAttack { get; set; }
        public int HeroHealth { get; set; }
        public int HeroHealLvl { get; set; }
        public IEnumerable<UpdateHeroSkillDto> HeroSkills { get; set; }
    }

    public class UpdateHeroSkillDto
    {
        public Guid SkillId { get; set; }
        public int Attack { get; set; }
    }
}
