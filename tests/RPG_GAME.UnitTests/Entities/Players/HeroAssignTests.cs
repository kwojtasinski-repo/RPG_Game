using FluentAssertions;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Players;
using System;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Players
{
    public class HeroAssignTests
    {
        private HeroAssign Act(string heroName, int health, int attack, int healLvl, IEnumerable<SkillHeroAssign> skills = null)
            => new HeroAssign(Guid.NewGuid(), heroName, health, attack, healLvl, skills);

        [Fact]
        public void should_create()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var healLvl = 1;

            var hero = Act(heroName, health, attack, healLvl);

            hero.Should().NotBeNull();
            hero.Health.Should().Be(health);
            hero.Attack.Should().Be(attack);
            hero.HealLvl.Should().Be(healLvl);
        }

        [Fact]
        public void should_create_with_skills()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var healLvl = 1;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };

            var hero = Act(heroName, health, attack, healLvl, skills);

            hero.Should().NotBeNull();
            hero.Health.Should().Be(health);
            hero.Attack.Should().Be(attack);
            hero.HealLvl.Should().Be(healLvl);
            hero.Skills.Should().NotBeEmpty();
            hero.Skills.Should().HaveCount(skills.Count);
        }

        [Fact]
        public void should_change_health()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var healLvl = 1;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var hero = Act(heroName, health, attack, healLvl, skills);
            var healthModified = 100;

            hero.ChangeHealth(healthModified);

            hero.Health.Should().Be(healthModified);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-25)]
        public void given_invalid_attack_should_throw_an_exception(int attack)
        {
            var heroName = "Hero#1";
            var health = 10;
            var healLvl = 1;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var expectedException = new HeroAssignAttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => Act(heroName, health, attack, healLvl, skills));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-35)]
        public void given_invalid_attack_when_change_value_should_throw_an_exception(int attackModified)
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var healLvl = 1;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var expectedException = new HeroAssignAttackCannotBeZeroOrNegativeException(attackModified);
            var hero = Act(heroName, health, attack, healLvl, skills);

            var exception = Record.Exception(() => hero.ChangeAttack(attackModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_attack()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var healLvl = 1;
            var attackModified = 100;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var hero = Act(heroName, health, attack, healLvl, skills);

            hero.ChangeAttack(attackModified);

            hero.Attack.Should().Be(attackModified);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-125)]
        public void given_invalid_heal_should_throw_an_exception(int healLvl)
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var expectedException = new HeroAssignHealLvlCannotBeZeroOrNegativeException(healLvl);

            var exception = Record.Exception(() => Act(heroName, health, attack, healLvl, skills));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-225)]
        public void given_invalid_heal_when_change_value_should_throw_an_exception(int healLvlModified)
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var healLvl = 1;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var hero = Act(heroName, health, attack, healLvl, skills);
            var expectedException = new HeroAssignHealLvlCannotBeZeroOrNegativeException(healLvlModified);

            var exception = Record.Exception(() => hero.ChangeHealLvl(healLvlModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_heal()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var healLvl = 1;
            var healLvlModified = 10;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var hero = Act(heroName, health, attack, healLvl, skills);

            hero.ChangeHealLvl(healLvlModified);

            hero.HealLvl.Should().Be(healLvlModified);
        }

        private static SkillHeroAssign CreateDefaultSkill()
        {
            return new SkillHeroAssign(Guid.NewGuid(), "skill#1", 100);
        }
    }
}
