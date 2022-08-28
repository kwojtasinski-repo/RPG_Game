using RPG_GAME.Application.DTO.Common;

namespace RPG_GAME.Application.DTO.Enemies
{
    public class EnemyDetailsDto
    {
        public Guid Id { get; set; }
        public string EnemyName { get; set; }
        public StateDto<int> BaseAttack { get; set; }
        public StateDto<int> BaseHealth { get; set; }
        public StateDto<int> BaseHealLvl { get; set; }
        public StateDto<decimal> Experience { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public IEnumerable<SkillEnemyDto> Skills { get; set; }
        public IEnumerable<Guid> MapsAssignedTo { get; set; }
    }
}
