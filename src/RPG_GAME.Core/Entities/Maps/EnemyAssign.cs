using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Maps;
using System;
using System.Collections.Generic;

namespace RPG_GAME.Core.Entities.Maps
{
    public class EnemyAssign
    {
        public Guid Id { get; private set; }
        public CharacterType Character => CharacterType.ENEMY;
        public string EnemyName { get; private set; }
        public int Attack { get; private set; }
        public int Health { get; private set; }
        public int HealLvl { get; private set; }
        public decimal Experience { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public IEnumerable<SkillEnemyAssign> Skills => _skills;

        private IEnumerable<SkillEnemyAssign> _skills = new List<SkillEnemyAssign>();

        public EnemyAssign(Guid id, string enemyName, int attack, int health, int healLvl, decimal experience,
            string difficulty, IEnumerable<SkillEnemyAssign> skills = null)
        {
            Id = id;
            EnemyName = enemyName;
            ChangeAttack(attack);
            ChangeHealth(health);
            ChangeHealLvl(healLvl);
            ChangeExperience(experience);

            var parsed = Enum.TryParse<Difficulty>(difficulty, out var difficultyType);

            if (!parsed)
            {
                throw new InvalidEnemyAssignDifficultyException(difficulty);
            }

            Difficulty = difficultyType;

            if (skills is not null)
            {
                _skills = skills;
            }
        }

        public void ChangeAttack(int attack) 
        {
            if (attack <= 0)
            {
                throw new AttackCannotBeZeroOrNegativeException(attack);
            }

            Attack = attack;
        }

        public void ChangeHealth(int health) 
        {
            if (health <= 0)
            {
                throw new HealthCannotBeZeroOrNegativeException(health);
            }

            Health = health;
        }

        public void ChangeHealLvl(int healLvl) 
        {
            if (healLvl <= 0)
            {
                throw new HealLvlCannotBeZeroOrNegativeException(healLvl);
            }

            HealLvl = healLvl;
        }

        public void ChangeExperience(decimal experience) 
        {
            if (experience <= 0)
            {
                throw new ExperienceCannotBeZeroOrNegativeException(experience);
            }

            Experience = experience;
        }

        internal void ChangeName(string enemyName)
        {
            if (string.IsNullOrWhiteSpace(enemyName))
            {
                return;
            }

            EnemyName = enemyName;
        }

        internal void ChangeDifficulty(Difficulty difficulty)
        {
            Difficulty = difficulty;
        }

        internal void ChangeSkills(IEnumerable<SkillEnemyAssign> skills)
        {
            if (skills is null)
            {
                return;
            }

            _skills = skills;
        }
    }
}
