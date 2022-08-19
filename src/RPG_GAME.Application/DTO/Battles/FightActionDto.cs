namespace RPG_GAME.Application.DTO.Battles
{
    public class FightActionDto
    {
        public Guid CharacterId { get; set; }
        public string Character { get; set; }
        public string Name { get; set; }
        public int DamageDealt { get; set; }
        public int Health { get; set; }
        public string AttackInfo { get; set; }
    }
}
