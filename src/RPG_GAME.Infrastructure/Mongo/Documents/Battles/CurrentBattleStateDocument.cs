namespace RPG_GAME.Infrastructure.Mongo.Documents.Battles
{
    internal class CurrentBattleStateDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid BattleId { get; set; }
        public Guid PlayerId { get; set; }
        public int PlayerCurrentHealth { get; set; }
        public int PlayerLevel { get; set; }
        public Guid EnemyId { get; set; }
        public int EnemyHealth { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
