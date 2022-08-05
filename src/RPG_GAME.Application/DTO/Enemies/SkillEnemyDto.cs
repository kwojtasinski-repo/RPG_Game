using RPG_GAME.Application.DTO.Common;

namespace RPG_GAME.Application.DTO.Enemies
{
    public class SkillEnemyDto : SkillEnemyDto<int>
    {
    }
    
    public class SkillEnemyDto<T>
        where T : struct
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public decimal Probability { get; set; }
        public IncreasingStateDto<T> IncreasingState { get; set; }
    }
}
