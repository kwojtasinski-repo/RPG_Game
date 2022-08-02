namespace RPG_GAME.Infrastructure.Mongo.Documents.Maps
{
    internal sealed class EnemyAssignDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.ENEMY;
        public string EnemyName { get; set; }
        public int BaseAttack { get; set; }
        public int BaseHealth { get; set; }
        public int BaseHealLvl { get; set; }
        public decimal Experience { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<SkillEnemyAssignDocument> Skills { get; set; }
    }
}
