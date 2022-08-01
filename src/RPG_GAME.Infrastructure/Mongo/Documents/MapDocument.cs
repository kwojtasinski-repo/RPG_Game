using RPG_GAME.Core.Entities;

namespace RPG_Game.Infrastructure.Mongo.Documents
{
    internal sealed class MapDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public IList<RequiredEnemyDocument> Enemies { get; set; }
    }
}
