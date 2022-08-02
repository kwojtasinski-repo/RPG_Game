namespace RPG_GAME.Infrastructure.Mongo.Documents.Heroes
{
    internal sealed class HeroDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.PLAYER;
        public string HeroName { get; set; }
        public StateDocument<int> Health { get; set; }
        public StateDocument<int> Attack { get; set; }
        public StateDocument<int> HealLvl { get; set; }
        public StateDocument<decimal> BaseRequiredExperience { get; set; }
        public IEnumerable<SkillHeroDocument> Skills { get; set; }
        public IEnumerable<Guid> PlayersAssignedTo { get; set; }
    }
}
