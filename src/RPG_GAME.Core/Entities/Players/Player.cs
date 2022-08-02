using System;

namespace RPG_GAME.Core.Entities.Players
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public HeroAssign Hero { get; set; }
        public int Level { get; set; }
        public decimal CurrentExp { get; set; }
        public decimal RequiredExp { get; set; }
        public Guid UserId { get; set; }
    }
}
