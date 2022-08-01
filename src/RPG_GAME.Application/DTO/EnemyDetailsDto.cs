using RPG_GAME.Core.Entities;

namespace RPG_GAME.Application.DTO
{
    public class EnemyDetailsDto
    {
        public Guid Id { get; set; }
        public string EnemyName { get; set; }
        public FieldDto<int> BaseAttack { get; set; }
        public FieldDto<int> BaseHealth { get; set; }
        public FieldDto<int> BaseHealLvl { get; set; }
        public FieldDto<decimal> Experience { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<SkillDetailsEnemyDto> Skills { get; set; }
    }
}
