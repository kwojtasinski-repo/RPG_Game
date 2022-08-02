namespace RPG_GAME.Application.DTO.Maps
{
    public class SkillEnemyAssignDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BaseAttack { get; set; }
        public decimal Probability { get; set; }
    }
}
