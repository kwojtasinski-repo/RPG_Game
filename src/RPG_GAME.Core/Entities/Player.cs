using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities
{
    public class Player : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.PLAYER;
        public string Name { get; set; }
        public Hero Hero { get; set; }
        public int Level { get; set; }
        public decimal CurrentExp { get; set; }
        public decimal RequiredExp { get; set; }
        public Guid UserId { get; set; }
    }
}
