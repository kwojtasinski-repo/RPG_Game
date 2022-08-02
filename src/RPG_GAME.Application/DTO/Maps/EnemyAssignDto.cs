namespace RPG_GAME.Application.DTO.Maps
{
    public class EnemyAssignDto
    {
        public Guid Id { get; set; }
        public string EnemyName { get; set; }
        public int BaseAttack { get; set; }
        public int BaseHealth { get; set; }
        public int BaseHealLvl { get; set; }
        public decimal Experience { get; set; }
        public string Difficulty { get; set; }
        public IEnumerable<SkillEnemyAssignDto> Skills { get; set; }
    }
}
