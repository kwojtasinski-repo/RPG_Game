using RPG_GAME.Infrastructure.Mongo.Documents.Players;

namespace RPG_GAME.Infrastructure.Mongo.Documents.Battles
{
    internal class BattleStateDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid BattleId { get; set; }
        public BattleStatus BattleStatus { get; set; }
        public PlayerDocument Player { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
