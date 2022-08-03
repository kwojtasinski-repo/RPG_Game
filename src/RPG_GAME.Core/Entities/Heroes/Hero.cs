using System;
using System.Collections.Generic;
using System.Linq;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Heroes;

namespace RPG_GAME.Core.Entities.Heroes
{
    public class Hero
    {
        public Guid Id { get; }
        public string HeroName { get; private set; }
        public State<int> Health { get; private set; }
        public State<int> Attack { get; private set; }
        public State<int> HealLvl { get; private set;  }
        public State<decimal> BaseRequiredExperience { get; private set; }
        public IEnumerable<SkillHero> Skills => _skills;
        public IEnumerable<Guid> PlayersAssignedTo => _playersAssignedTo;

        private IList<Guid> _playersAssignedTo = new List<Guid>();
        private IList<SkillHero> _skills = new List<SkillHero>();

        public Hero(Guid id, string heroName, State<int> health, State<int> attack, State<int> healLvl, State<decimal> baseRequiredExperience,
            IEnumerable<SkillHero> skills = null, IEnumerable<Guid> playersAssignedTo = null)
        {
            Id = id;
            ChangeHeroName(heroName);
            ChangeAttack(attack);
            ChangeHealth(health);
            ChangeHealLvl(healLvl);
            ChangeBaseRequiredExperience(baseRequiredExperience);

            if (skills is not null)
            {
                foreach(var skill in skills)
                {
                    AddSkill(skill);
                }
            }

            if (playersAssignedTo is not null)
            {
                foreach (var playerAssignedTo in playersAssignedTo)
                {
                    AddPlayer(playerAssignedTo);
                }
            }
        }

        public void ChangeHeroName(string heroName)
        {
            if (string.IsNullOrWhiteSpace(heroName))
            {
                throw new InvalidHeroNameException();
            }

            if (heroName.Length < 3)
            {
                throw new TooShortHeroNameException(heroName);
            }

            HeroName = heroName;
        }

        public void ChangeHealth(State<int> health)
        {
            if (health is null)
            {
                throw new InvalidHeroHealthException();
            }

            if (health.Value < 0)
            {
                throw new HeroHealthCannotBeNegativeException(health.Value);
            }

            if (health.IncreasingState.Value < 0)
            {
                throw new HeroHealthIncreasingCannotBeNegativeException(health.IncreasingState.Value);
            }

            Health = health;
        }

        public void ChangeAttack(State<int> attack)
        {
            if (attack is null)
            {
                throw new InvalidHeroAttackException();
            }

            if (attack.Value < 0)
            {
                throw new HeroAttackCannotBeNegativeException(attack.Value);
            }

            if (attack.IncreasingState.Value < 0)
            {
                throw new HeroAttackIncreasingCannotBeNegativeException(attack.IncreasingState.Value);
            }

            Attack = attack;
        }

        public void ChangeHealLvl(State<int> healLvl)
        {
            if (healLvl is null)
            {
                throw new InvalidHeroHealLvlException();
            }

            if (healLvl.Value < 0)
            {
                throw new HeroHealLvlCannotBeNegativeException(healLvl.Value);
            }

            if (healLvl.IncreasingState.Value < 0)
            {
                throw new HeroHealLvlIncreasingCannotBeNegativeException(healLvl.IncreasingState.Value);
            }

            HealLvl = healLvl;
        }

        public void ChangeBaseRequiredExperience(State<decimal> baseRequiredExperience)
        {
            if (baseRequiredExperience is null)
            {
                throw new InvalidHeroBaseRequiredExperienceException();
            }

            if (baseRequiredExperience.Value < 0)
            {
                throw new HeroBaseReqExpCannotBeNegativeException(baseRequiredExperience.Value);
            }

            if (baseRequiredExperience.IncreasingState.Value < 0)
            {
                throw new HeroBaseReqExpIncreasingCannotBeNegativeException(baseRequiredExperience.IncreasingState.Value);
            }

            BaseRequiredExperience = baseRequiredExperience;
        }

        internal void AddPlayer(Guid playerId) // allow add only by domain service and react on events
        {
            if (playerId == default)
            {
                throw new InvalidPlayerIdException();
            }

            var playerIdExists = _playersAssignedTo.SingleOrDefault(p => p == playerId);

            if (playerIdExists != default)
            {
                throw new PlayerAlreadyExistsException(playerId);
            }

            _playersAssignedTo.Add(playerId);
        }

        internal void RemovePlayer(Guid playerId) // allow add only by domain service and react on events
        {
            if (playerId == default)
            {
                throw new InvalidPlayerIdException();
            }

            var playerIdExists = _playersAssignedTo.SingleOrDefault(p => p == playerId);

            if (playerIdExists == default)
            {
                throw new PlayerDoesntExistsException(playerId);
            }

            _playersAssignedTo.Remove(playerIdExists);
        }

        public void AddSkill(SkillHero skillHero)
        {
            if (skillHero is null)
            {
                throw new InvalidSkillHeroException();
            }

            var skillHeroExists = _skills.SingleOrDefault(s => s.Id == skillHero.Id);

            if (skillHeroExists is not null)
            {
                throw new SkillHeroAlreadyExistsException(skillHero.Id, skillHero.Name);
            }

            _skills.Add(skillHero);
        }

        public void RemoveSkill(SkillHero skillHero)
        {
            if (skillHero is null)
            {
                throw new InvalidSkillHeroException();
            }

            var skillHeroExists = _skills.SingleOrDefault(s => s.Id == skillHero.Id);

            if (skillHeroExists is null)
            {
                throw new SkillHeroDoesntExistsException(skillHero.Id, skillHero.Name);
            }

            _skills.Remove(skillHeroExists);
        }
    }
}
