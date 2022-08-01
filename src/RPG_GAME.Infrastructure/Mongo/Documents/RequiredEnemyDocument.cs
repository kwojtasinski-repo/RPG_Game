namespace RPG_Game.Infrastructure.Mongo.Documents
{
    internal class RequiredEnemyDocument
    {
        public EnemyDocument Enemy { get; set; }
        public int Quantity { get; set; }
    }
}
