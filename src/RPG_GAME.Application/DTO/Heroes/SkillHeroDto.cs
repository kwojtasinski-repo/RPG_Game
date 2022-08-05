using RPG_GAME.Application.DTO.Common;

namespace RPG_GAME.Application.DTO.Heroes
{
    public class SkillHeroDto : SkillHeroDto<int>
    {
    }
    
    public class SkillHeroDto<T>
        where T : struct
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public IncreasingStateDto<T> IncreasingState { get; set; }
    }
}
