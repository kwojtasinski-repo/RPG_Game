using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities
{
    public class Hero : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.PLAYER;
        public string HeroName { get; set; }
        public Field<int> Health { get; set; }
        public Field<int> Attack { get; set; }
        public Field<int> HealLvl { get; set; }
        public Field<decimal> BaseRequiredExperience { get; set; }
        public IEnumerable<Skill> Skills { get; set; }
    }
}
