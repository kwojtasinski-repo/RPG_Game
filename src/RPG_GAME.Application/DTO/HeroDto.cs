namespace RPG_GAME.Application.DTO
{
    public class HeroDto
    {
        public Guid Id { get; set; }
        public string HeroName { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int HealLvl { get; set; }
    }
}
