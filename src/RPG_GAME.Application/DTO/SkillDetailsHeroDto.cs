namespace RPG_GAME.Application.DTO
{
    public class SkillDetailsHeroDto : SkillDetailsHeroDto<int>
    {
    }
    
    public class SkillDetailsHeroDto<T>
        where T : struct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public IncreasingStatsDto<T> IncreasingStats { get; set; }
    }
}
