using System;

namespace RPG_GAME.Core.NewMappings
{
    // TODO: Add Base Entity?
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int HealLvl { get; set; }
        public int Level { get; set; }
        public int CurrentExp { get; set; }
        public int RequiredExp { get; set; }
        public string Profession { get; set; }
        public Guid UserId { get; set; }
    }
}
