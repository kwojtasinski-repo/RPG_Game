namespace RPG_GAME.Application.DTO.Battles
{
    public class BattleStatusDto
    {
        public Guid BattleId { get; set; }
        public Guid PlayerId { get; set; }
        public int PlayerHealth { get; set; }
        public bool PlayerIsDead => PlayerHealth < 0;
        public Guid EnemyId { get; set; }
        public int EnemyHealth { get; set; }
        public bool EnemyIsDead => EnemyHealth < 0;
    }
}
