namespace RPG_GAME.Infrastructure.Mongo.Documents.Maps
{
    internal sealed class EnemiesDocument
    {
        public EnemyAssignDocument Enemy { get; set; }
        public int Quantity { get; set; }
    }
}
