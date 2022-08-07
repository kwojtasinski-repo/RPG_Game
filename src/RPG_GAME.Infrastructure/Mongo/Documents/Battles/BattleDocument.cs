namespace RPG_GAME.Infrastructure.Mongo.Documents.Battles
{
    internal class BattleDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public Guid UserId { get; set; }
        public BattleInfo BattleInfo { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<BattleStateDocument> BattleStates { get; set; }
    }
}
