using RPG_GAME.Infrastructure.Mongo.Documents.Battles.Actions;

namespace RPG_GAME.Infrastructure.Mongo.Documents.Battles
{
    internal class BattleEventDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public Guid BattleId { get; set; }
        public FightActionDocument Action { get; set; }
        public DateTime Created { get; set; }
    }
}
