using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.Core.Entities.Enemies
{
    public class Enemy
    {
        public Guid Id { get; }
        public string EnemyName { get; private set; }
        public State<int> BaseAttack { get; private set; }
        public State<int> BaseHealth { get; private set; }
        public State<int> BaseHealLvl { get; private set; }
        public State<decimal> Experience { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public IEnumerable<SkillEnemy> Skills => _skills;
        public IEnumerable<Guid> MapsAssignedTo => _mapsAssignedTo;

        private IList<SkillEnemy> _skills = new List<SkillEnemy>();
        private IList<Guid> _mapsAssignedTo = new List<Guid>();

        public Enemy(Guid id, string enemyName, State<int> health, State<int> attack, State<int> healLvl, State<decimal> experience,
            string difficulty, IEnumerable<SkillEnemy> skills = null, IEnumerable<Guid> mapsAssignedTo = null)
        {
            Id = id;
            ChangeEnemyName(enemyName);
            ChangeHealth(health);
            ChangeAttack(attack);
            ChangeHealLvl(healLvl);
            ChangeExperience(experience);
            ChangeDifficulty(difficulty);

            if (skills is not null)
            {
                foreach (var skill in skills)
                {
                    AddSkill(skill);
                }
            }

            if (mapsAssignedTo is not null)
            {
                foreach (var mapAssignedTo in mapsAssignedTo)
                {
                    AddMap(mapAssignedTo);
                }
            }
        }

        public void ChangeEnemyName(string enemyName)
        {
            if (string.IsNullOrWhiteSpace(enemyName))
            {
                throw new InvalidEnemyNameException();
            }

            if (enemyName.Length < 3)
            {
                throw new TooShortEnemyNameException(enemyName);
            }

            EnemyName = enemyName;
        }

        public void ChangeHealth(State<int> health)
        {
            if (health is null)
            {
                throw new InvalidEnemyHealthException();
            }

            if (health.Value <= 0)
            {
                throw new EnemyHealthCannotBeZeroOrNegativeException(health.Value);
            }

            if (health.IncreasingState.Value < 0)
            {
                throw new EnemyHealthIncreasingCannotBeNegativeException(health.IncreasingState.Value);
            }

            BaseHealth = health;
        }

        public void ChangeAttack(State<int> attack)
        {
            if (attack is null)
            {
                throw new InvalidEnemyAttackException();
            }

            if (attack.Value <= 0)
            {
                throw new EnemyAttackCannotBeZeroOrNegativeException(attack.Value);
            }

            if (attack.IncreasingState.Value < 0)
            {
                throw new EnemyAttackIncreasingCannotBeNegativeException(attack.IncreasingState.Value);
            }

            BaseAttack = attack;
        }

        public void ChangeHealLvl(State<int> healLvl)
        {
            if (healLvl is null)
            {
                throw new InvalidEnemyHealLvlException();
            }

            if (healLvl.Value <= 0)
            {
                throw new EnemyHealLvlCannotBeZeroOrNegativeException(healLvl.Value);
            }

            if (healLvl.IncreasingState.Value < 0)
            {
                throw new EnemyHealLvlIncreasingCannotBeNegativeException(healLvl.IncreasingState.Value);
            }

            BaseHealLvl = healLvl;
        }

        public void ChangeDifficulty(string difficulty)
        {
            var parsed = Enum.TryParse<Difficulty>(difficulty, out var difficultyType);

            if (!parsed)
            {
                throw new InvalidEnemyDifficultyException(difficulty);
            }

            Difficulty = difficultyType;
        }

        public void ChangeExperience(State<decimal> experience)
        {
            if (experience is null)
            {
                throw new InvalidEnemyExperienceException();
            }

            if (experience.Value <= 0)
            {
                throw new EnemyExperienceCannotBeZeroOrNegativeException(experience.Value);
            }

            if (experience.IncreasingState.Value < 0)
            {
                throw new EnemyExperienceIncreasingCannotBeNegativeException(experience.IncreasingState.Value);
            }

            Experience = experience;
        }

        internal void AddMap(Guid mapId) // allow add only by domain service and react on events
        {
            if (mapId == default)
            {
                throw new InvalidMapIdException();
            }

            var mapIdExists = _mapsAssignedTo.SingleOrDefault(p => p == mapId);

            if (mapIdExists != default)
            {
                throw new MapAlreadyExistsException(mapId);
            }

            _mapsAssignedTo.Add(mapId);
        }

        internal void RemoveMap(Guid mapId) // allow add only by domain service and react on events
        {
            if (mapId == default)
            {
                throw new InvalidMapIdException();
            }

            var mapIdExists = _mapsAssignedTo.SingleOrDefault(p => p == mapId);

            if (mapIdExists == default)
            {
                throw new MapDoesntExistsException(mapId);
            }

            _mapsAssignedTo.Remove(mapIdExists);
        }

        public void AddSkill(SkillEnemy skillEnemy)
        {
            if (skillEnemy is null)
            {
                throw new InvalidSkillEnemyException();
            }

            var skillEnemyExists = _skills.SingleOrDefault(s => s.Id == skillEnemy.Id);

            if (skillEnemyExists is not null)
            {
                throw new SkillEnemyAlreadyExistsException(skillEnemy.Id, skillEnemy.Name);
            }

            _skills.Add(skillEnemy);
        }

        public void RemoveSkill(SkillEnemy skillEnemy)
        {
            if (skillEnemy is null)
            {
                throw new InvalidSkillEnemyException();
            }

            var skillEnemyExists = _skills.SingleOrDefault(s => s.Id == skillEnemy.Id);

            if (skillEnemyExists is null)
            {
                throw new SkillEnemyDoesntExistsException(skillEnemy.Id, skillEnemy.Name);
            }

            _skills.Remove(skillEnemyExists);
        }
    }
}
