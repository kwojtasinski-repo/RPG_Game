using RPG_GAME.Core.Entities.Common;
using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities.Players
{
    public class HeroAssign
    {
        public Guid Id { get; set; }
        public CharacterType Character => CharacterType.PLAYER;
        public string HeroName { get; set; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int HealLvl { get; set; }
        public decimal BaseRequiredExperience { get; set; }
        public IEnumerable<SkillHeroAssign> Skills { get; set; }
    }
}
