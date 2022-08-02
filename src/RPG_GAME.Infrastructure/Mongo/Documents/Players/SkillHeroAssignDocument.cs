namespace RPG_GAME.Infrastructure.Mongo.Documents.Players
{
    internal sealed class SkillHeroAssignDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BaseAttack { get; set; }
    }
}
