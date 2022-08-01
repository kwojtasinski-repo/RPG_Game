using RPG_GAME.Core.Entities;

namespace RPG_Game.Infrastructure.Mongo.Documents
{
    internal sealed class EnemyDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character;
        public string EnemyName { get; set; }
        public Field<int> BaseAttack { get; set; }
        public Field<int> BaseHealth { get; set; }
        public Field<int> BaseHealLvl { get; set; }
        public Field<decimal> Experience { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<SkillEnemy> Skills { get; set; }
    }
}
