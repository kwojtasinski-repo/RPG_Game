using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Players;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.Core.Entities.Players
{
    public class HeroAssign
    {
        public Guid Id { get; }
        public CharacterType Character => CharacterType.PLAYER;
        public string HeroName { get; private set; }
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public IEnumerable<SkillHeroAssign> Skills => _skills;

        private IList<SkillHeroAssign> _skills = new List<SkillHeroAssign>();

        public HeroAssign(Guid id, string heroName, int health, int attack, IEnumerable<SkillHeroAssign> skills = null)
        {
            Id = id;
            HeroName = heroName;
            ChangeHealth(health);
            ChangeAttack(attack);

            if (skills is not null)
            {
                _skills = new List<SkillHeroAssign>(skills);
            }
        }

        public void ChangeHealth(int health) 
        {
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

        internal void ChangeHeroName(string heroName)
        {
            if (string.IsNullOrWhiteSpace(heroName))
            {
                return;
            }

            HeroName = heroName;
        }

        internal void ChangeSkills(IEnumerable<SkillHeroAssign> skills)
        {
            if (skills is null)
            {
                return;
            }

            _skills = skills.ToList();
        }

        public static class Action
        {
            public const string BASE_ATTACK = "attack";
        }
    }
}
