namespace RPG_GAME.Infrastructure.Mongo.Documents.Maps
{
    internal sealed class SkillEnemyAssignDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BaseAttack { get; set; }
        public decimal Probability { get; set; }
    }
}
