using System;
using System.Collections.Generic;
using RPG_GAME.Core.Entities.Common;

namespace RPG_GAME.Core.Entities.Heroes
{
    public class Hero : IIdentifiable<Guid>
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.PLAYER;
        public string HeroName { get; set; }
        public State<int> Health { get; set; }
        public State<int> Attack { get; set; }
        public State<int> HealLvl { get; set; }
        public State<decimal> BaseRequiredExperience { get; set; }
        public IEnumerable<SkillHero> Skills { get; set; }
    }
}
