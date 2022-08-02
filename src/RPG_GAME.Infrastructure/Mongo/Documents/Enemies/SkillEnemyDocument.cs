namespace RPG_GAME.Infrastructure.Mongo.Documents.Enemies
{
    internal class SkillEnemyDocument : SkillEnemyDocument<int>
    {
    }

    internal class SkillEnemyDocument<T> : IIdentifiable<Guid>
        where T : struct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public decimal Probability { get; set; }
        public IncreasingStateDocument<T> IncreasingState { get; set; }
    }
}