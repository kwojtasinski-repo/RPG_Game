using FluentAssertions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Exceptions.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Heroes
{
    public class HeroTests
    {
        private Hero Act(string heroName, int health, int attack, int healLvl, decimal experience,
            IEnumerable<SkillHero> skills = null)
                => new Hero(Guid.NewGuid(), heroName, new State<int>(health, DefaultIncreasingState<int>()), new State<int>(attack, DefaultIncreasingState<int>()),
                    new State<int>(healLvl, DefaultIncreasingState<int>()), new State<decimal>(experience, DefaultIncreasingState<decimal>()),
                    skills);

        private Hero Act(string heroName, int health, IncreasingState<int> healthIncreasing, int attack, IncreasingState<int> attackIncreasing, int healLvl, IncreasingState<int> healLvlIncreasing,
            decimal experience, IncreasingState<decimal> experienceIncreasing, IEnumerable<SkillHero> skills = null)
                => new Hero(Guid.NewGuid(), heroName, new State<int>(health, healthIncreasing), new State<int>(attack, attackIncreasing),
                    new State<int>(healLvl, healLvlIncreasing), new State<decimal>(experience, experienceIncreasing), skills);

        [Fact]
        public void should_create_hero()
        {
            var name = "Hero #1";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;

            var hero = Act(name, health, attack, heal, experience);

            hero.Should().NotBeNull();
            hero.Id.Should().NotBe(Guid.Empty);
            hero.HeroName.Should().Be(name);
            hero.Attack.Value.Should().Be(attack);
            hero.Health.Value.Should().Be(health);
            hero.HealLvl.Value.Should().Be(heal);
            hero.BaseRequiredExperience.Value.Should().Be(experience);
        }

        [Fact]
        public void should_create_hero_with_skills()
        {
            var name = "Hero #1";
            var health = 100;
            var healthIncrease = new IncreasingState<int>(25, "ADDITIVE");
            var attack = 20;
            var attackIncrease = new IncreasingState<int>(5, "ADDITIVE");
            var heal = 5;
            var healIncrease = new IncreasingState<int>(2, "ADDITIVE");
            var experience = 50M;
            var experienceIncrease = new IncreasingState<decimal>(15, "ADDITIVE");
            var skills = new List<SkillHero> { new SkillHero(Guid.NewGuid(), "Name#1", 30, new IncreasingState<int>(10, "ADDITIVE")) };

            var hero = Act(name, health, healthIncrease, attack, attackIncrease, heal, healIncrease,
                experience, experienceIncrease, skills);

            hero.Should().NotBeNull();
            hero.Id.Should().NotBe(Guid.Empty);
            hero.HeroName.Should().Be(name);
            hero.Attack.Value.Should().Be(attack);
            hero.Attack.IncreasingState.Value.Should().Be(attackIncrease.Value);
            hero.HealLvl.Value.Should().Be(heal);
            hero.HealLvl.IncreasingState.Value.Should().Be(healIncrease.Value);
            hero.Health.Value.Should().Be(health);
            hero.Health.IncreasingState.Value.Should().Be(healthIncrease.Value);
            hero.BaseRequiredExperience.Value.Should().Be(experience);
            hero.BaseRequiredExperience.IncreasingState.Value.Should().Be(experienceIncrease.Value);
            hero.Skills.Should().NotBeNull();
            hero.Skills.Should().NotBeEmpty();
            hero.Skills.First().Name.Should().Be(skills.First().Name);
        }

        [Fact]
        public void given_empty_name_should_throw_an_exception()
        {
            var name = "";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var expectedException = new InvalidHeroNameException();

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_too_short_name_should_throw_an_exception()
        {
            var name = "t";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var expectedException = new TooShortHeroNameException(name);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_health_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var expectedException = new InvalidHeroHealthException();

            var exception = Record.Exception(() => new Hero(Guid.NewGuid(), name, null, new State<int>(attack, DefaultIncreasingState<int>()),
                    new State<int>(heal, DefaultIncreasingState<int>()), new State<decimal>(experience, DefaultIncreasingState<decimal>())));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_health_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 0;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var expectedException = new HeroHealthCannotBeZeroOrNegativeException(health);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_health_should_change_value()
        {
            var name = "Hero #1";
            var health = 100;
            var healthModified = 120;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var hero = Act(name, health, attack, heal, experience);

            hero.ChangeHealth(new State<int>(healthModified, DefaultIncreasingState<int>()));

            hero.Health.Value.Should().Be(healthModified);
        }

        [Fact]
        public void given_null_attack_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 200;
            var heal = 5;
            var experience = 50M;
            var expectedException = new InvalidHeroAttackException();

            var exception = Record.Exception(() => new Hero(Guid.NewGuid(), name, new State<int>(health, DefaultIncreasingState<int>()), null,
                    new State<int>(heal, DefaultIncreasingState<int>()), new State<decimal>(experience, DefaultIncreasingState<decimal>())));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_attack_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 120;
            var attack = 0;
            var heal = 5;
            var experience = 50M;
            var expectedException = new HeroAttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_attack_should_change_value()
        {
            var name = "Hero #1";
            var health = 100;
            var attack = 20;
            var attackModified = 20;
            var heal = 5;
            var experience = 50M;
            var hero = Act(name, health, attack, heal, experience);

            hero.ChangeAttack(new State<int>(attackModified, DefaultIncreasingState<int>()));

            hero.Attack.Value.Should().Be(attackModified);
        }

        [Fact]
        public void given_null_heal_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 5;
            var attack = 20;
            var experience = 50M;
            var expectedException = new InvalidHeroHealLvlException();

            var exception = Record.Exception(() => new Hero(Guid.NewGuid(), name, new State<int>(health, DefaultIncreasingState<int>()), new State<int>(attack, DefaultIncreasingState<int>()),
                    null, new State<decimal>(experience, DefaultIncreasingState<decimal>())));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_heal_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 210;
            var attack = 20;
            var heal = 0;
            var experience = 50M;
            var expectedException = new HeroHealLvlCannotBeZeroOrNegativeException(heal);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_heal_should_change_value()
        {
            var name = "Hero #1";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var healModified = 120;
            var experience = 50M;
            var hero = Act(name, health, attack, heal, experience);

            hero.ChangeHealLvl(new State<int>(healModified, DefaultIncreasingState<int>()));

            hero.HealLvl.Value.Should().Be(healModified);
        }

        [Fact]
        public void given_null_experience_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var expectedException = new InvalidHeroBaseRequiredExperienceException();

            var exception = Record.Exception(() => new Hero(Guid.NewGuid(), name, new State<int>(health, DefaultIncreasingState<int>()), new State<int>(attack, DefaultIncreasingState<int>()),
                    new State<int>(heal, DefaultIncreasingState<int>()), null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_experience_should_throw_an_exception()
        {
            var name = "AbCDeF";
            var health = 210;
            var attack = 20;
            var heal = 5;
            var experience = 0M;
            var expectedException = new HeroBaseReqExpCannotBeZeroOrNegativeException(experience);

            var exception = Record.Exception(() => Act(name, health, attack, heal, experience));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_experience_should_change_value()
        {
            var name = "Hero #1";
            var health = 100;
            var attack = 20;
            var heal = 5;
            var experience = 50M;
            var experienceModified = 50M;
            var hero = Act(name, health, attack, heal, experience);

            hero.ChangeBaseRequiredExperience(new State<decimal>(experienceModified, DefaultIncreasingState<decimal>()));

            hero.BaseRequiredExperience.Value.Should().Be(experienceModified);
        }

        [Fact]
        public void given_valid_skill_should_add()
        {
            var hero = CreateDefaultHero();
            var skill = new SkillHero(Guid.NewGuid(), "Name#1", 100, DefaultIncreasingState<int>());

            hero.AddSkill(skill);

            hero.Skills.Should().NotBeEmpty();
            hero.Skills.Should().HaveCount(1);
        }

        [Fact]
        public void given_same_as_added_skill_should_throw_an_exception()
        {
            var hero = CreateDefaultHero();
            var skill = new SkillHero(Guid.NewGuid(), "Name#1", 100, DefaultIncreasingState<int>());
            var skill2 = new SkillHero(skill.Id, "Name#1", 100, DefaultIncreasingState<int>());
            hero.AddSkill(skill);
            var expectedException = new SkillHeroAlreadyExistsException(skill2.Id, skill2.Name);

            var exception = Record.Exception(() => hero.AddSkill(skill2));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_skill_when_add_should_throw_an_exception()
        {
            var hero = CreateDefaultHero();
            var expectedException = new InvalidSkillHeroException();

            var exception = Record.Exception(() => hero.AddSkill(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_skill_should_remove()
        {
            var hero = CreateDefaultHero();
            var skill = new SkillHero(Guid.NewGuid(), "Name#1", 100, DefaultIncreasingState<int>());
            hero.AddSkill(skill);

            hero.RemoveSkill(new SkillHero(skill.Id, skill.Name, skill.BaseAttack, skill.IncreasingState));

            hero.Skills.Should().BeEmpty();
        }

        [Fact]
        public void given_not_existed_skill_when_delete_should_throw_an_exception()
        {
            var hero = CreateDefaultHero();
            var skill = new SkillHero(Guid.NewGuid(), "Name#1", 100, DefaultIncreasingState<int>());
            var expectedException = new SkillHeroDoesntExistsException(skill.Id, skill.Name);

            var exception = Record.Exception(() => hero.RemoveSkill(skill));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_skill_should_throw_an_exception()
        {
            var hero = CreateDefaultHero();
            var expectedException = new InvalidSkillHeroException();

            var exception = Record.Exception(() => hero.RemoveSkill(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_default_player_id_when_add_should_throw_an_exception()
        {
            var hero = CreateDefaultHero();
            var expectedException = new InvalidPlayerIdException();

            var exception = Record.Exception(() => hero.AddPlayer(default));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_existed_player_id_when_add_should_throw_an_exception()
        {
            var hero = CreateDefaultHero();
            var playerId = Guid.NewGuid();
            hero.AddPlayer(playerId);
            var expectedException = new PlayerAlreadyExistsException(playerId);

            var exception = Record.Exception(() => hero.AddPlayer(playerId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_add_player()
        {
            var hero = CreateDefaultHero();
            var playerId = Guid.NewGuid();

            hero.AddPlayer(playerId);

            hero.PlayersAssignedTo.Should().NotBeEmpty();
            hero.PlayersAssignedTo.Should().HaveCount(1);
            hero.PlayersAssignedTo.Should().Contain(playerId);
        }

        [Fact]
        public void should_delete_player()
        {
            var hero = CreateDefaultHero();
            var playerId = Guid.NewGuid();
            hero.AddPlayer(playerId);

            hero.RemovePlayer(playerId);

            hero.PlayersAssignedTo.Should().BeEmpty();
        }

        [Fact]
        public void given_not_existed_player_id_when_delete_should_throw_an_exception()
        {
            var hero = CreateDefaultHero();
            var playerId = Guid.NewGuid();
            var expectedException = new PlayerDoesntExistsException(playerId);

            var exception = Record.Exception(() => hero.RemovePlayer(playerId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_default_map_id_when_delete_should_throw_an_exception()
        {
            var player = CreateDefaultHero();
            var expectedException = new InvalidPlayerIdException();

            var exception = Record.Exception(() => player.RemovePlayer(default));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private static Hero CreateDefaultHero()
        {
            return new Hero(Guid.NewGuid(), "Hero", new State<int>(100, DefaultIncreasingState<int>()), new State<int>(100, DefaultIncreasingState<int>()),
                    new State<int>(10, DefaultIncreasingState<int>()), new State<decimal>(1000, DefaultIncreasingState<decimal>()));
        }

        private static IncreasingState<T> DefaultIncreasingState<T>()
            where T : struct
        {
            T value = default;

            if (typeof(T) == typeof(decimal))
            {
                object val = 1M;
                value = (T)val;
            }

            if (typeof(T) == typeof(int))
            {
                object val = 1;
                value = (T)val;
            }

            return new IncreasingState<T>(value, "ADDITIVE");
        }
    }
}
