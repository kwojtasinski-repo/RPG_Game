namespace RPG_GAME.Infrastructure.Mongo.Documents.Players
{
    internal class PlayerDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public HeroAssignDocument Hero { get; set; }
        public int Level { get; set; }
        public decimal CurrentExp { get; set; }
        public decimal RequiredExp { get; set; }
        public Guid UserId { get; set; }
    }
}
