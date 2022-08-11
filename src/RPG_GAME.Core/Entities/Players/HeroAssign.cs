using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Players;
using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities.Players
{
    public class HeroAssign
    {
        public Guid Id { get; }
        public CharacterType Character => CharacterType.PLAYER;
        public string HeroName { get; }
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public int HealLvl { get; private set; }
        public IEnumerable<SkillHeroAssign> Skills => _skills;

        private IList<SkillHeroAssign> _skills = new List<SkillHeroAssign>();

        public HeroAssign(Guid id, string heroName, int health, int attack, int healLvl, IEnumerable<SkillHeroAssign> skills = null)
        {
            Id = id;
            HeroName = heroName;
            ChangeHealth(health);
            ChangeAttack(attack);
            ChangeHealLvl(healLvl);

            if (skills is not null)
            {
                _skills = new List<SkillHeroAssign>(skills);
            }
        }

        public void ChangeHealth(int health) 
        {
            if (health is <= 0)
            {
                throw new HeroAssignHealthCannotBeZeroOrNegativeException(health);
            }

            Health = health;
        }

        public void ChangeAttack(int attack) 
        {
            if (attack is <= 0)
            {
                throw new HeroAssignAttackCannotBeZeroOrNegativeException(attack);
            }

            Attack = attack;
        }

        public void ChangeHealLvl(int healLvl) 
        {
            if (healLvl is <= 0)
            {
                throw new HeroAssignHealLvlCannotBeZeroOrNegativeException(healLvl);
            }

            HealLvl = healLvl;
        }
    }
}
