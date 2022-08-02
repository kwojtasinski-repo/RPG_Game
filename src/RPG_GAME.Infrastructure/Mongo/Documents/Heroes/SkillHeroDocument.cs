namespace RPG_GAME.Infrastructure.Mongo.Documents.Heroes
{
    internal class SkillHeroDocument : SkillHeroDocument<int>
    {
    }

    internal class SkillHeroDocument<T> : IIdentifiable<Guid>
        where T : struct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public IncreasingStateDocument<T> IncreasingState { get; set; }
    }
}
