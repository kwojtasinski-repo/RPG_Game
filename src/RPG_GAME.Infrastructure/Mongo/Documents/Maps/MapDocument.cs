namespace RPG_GAME.Infrastructure.Mongo.Documents.Maps
{
    internal sealed class MapDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<EnemiesDocument> Enemies { get; set; }
    }
}
