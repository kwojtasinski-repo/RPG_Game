using RPG_GAME.Core.Entities;

namespace RPG_Game.Infrastructure.Mongo.Documents
{
    internal class PlayerDocument : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public HeroDocument Hero { get; set; }
        public int Level { get; set; }
        public decimal CurrentExp { get; set; }
        public decimal RequiredExp { get; set; }
        public Guid UserId { get; set; }
    }
}
