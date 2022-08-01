namespace RPG_GAME.Application.DTO
{
    public class SkillEnemyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BaseAttack { get; set; }
        public decimal Probability { get; set; }
    }
}
