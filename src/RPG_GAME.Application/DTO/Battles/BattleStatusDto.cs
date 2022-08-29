namespace RPG_GAME.Application.DTO.Battles
{
    public class BattleStatusDto
    {
        public Guid BattleId { get; set; }
        public Guid PlayerId { get; set; }
        public int PlayerHealth { get; set; }
        public Guid EnemyId { get; set; }
        public int EnemyHealth { get; set; }
    }
}
