using RPG_GAME.Core.Entities;

namespace RPG_Game.Infrastructure.Mongo.Documents
{
    internal sealed class HeroDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character;
        public string HeroName { get; set; }
        public Field<int> Health { get; set; }
        public Field<int> Attack { get; set; }
        public Field<int> HealLvl { get; set; }
        public Field<decimal> BaseRequiredExperience { get; set; }
        public IEnumerable<SkillHero> Skills { get; set; }
    }
}
