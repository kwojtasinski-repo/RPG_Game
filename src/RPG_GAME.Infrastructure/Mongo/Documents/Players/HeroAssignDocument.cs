namespace RPG_GAME.Infrastructure.Mongo.Documents.Players
{
    internal sealed class HeroAssignDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.PLAYER;
        public string HeroName { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int HealLvl { get; set; }
        public decimal BaseRequiredExperience { get; set; }
        public IEnumerable<SkillHeroAssignDocument> Skills { get; set; }
    }
}
