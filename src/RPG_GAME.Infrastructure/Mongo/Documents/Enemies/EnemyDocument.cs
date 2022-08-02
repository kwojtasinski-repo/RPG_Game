namespace RPG_GAME.Infrastructure.Mongo.Documents.Enemies
{
    internal sealed class EnemyDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.ENEMY;
        public string EnemyName { get; set; }
        public StateDocument<int> BaseAttack { get; set; }
        public StateDocument<int> BaseHealth { get; set; }
        public StateDocument<int> BaseHealLvl { get; set; }
        public StateDocument<decimal> Experience { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<SkillEnemyDocument> Skills { get; set; }
    }
}
