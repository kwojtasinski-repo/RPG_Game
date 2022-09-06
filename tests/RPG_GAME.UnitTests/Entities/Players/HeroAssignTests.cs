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
        private HeroAssign Act(string heroName, int health, int attack, IEnumerable<SkillHeroAssign> skills = null)
            => new HeroAssign(Guid.NewGuid(), heroName, health, attack, skills);

        [Fact]
        public void should_create()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;

            var hero = Act(heroName, health, attack);

            hero.Should().NotBeNull();
            hero.Health.Should().Be(health);
            hero.Attack.Should().Be(attack);
        }

        [Fact]
        public void should_create_with_skills()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };

            var hero = Act(heroName, health, attack, skills);

            hero.Should().NotBeNull();
            hero.Health.Should().Be(health);
            hero.Attack.Should().Be(attack);
            hero.Skills.Should().NotBeEmpty();
            hero.Skills.Should().HaveCount(skills.Count);
        }

        [Fact]
        public void should_change_health()
        {
            var heroName = "Hero#1";
            var health = 10;
            var attack = 10;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var hero = Act(heroName, health, attack, skills);
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
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var expectedException = new HeroAssignAttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => Act(heroName, health, attack, skills));

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
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var expectedException = new HeroAssignAttackCannotBeZeroOrNegativeException(attackModified);
            var hero = Act(heroName, health, attack, skills);

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
            var attackModified = 100;
            var skills = new List<SkillHeroAssign> { CreateDefaultSkill() };
            var hero = Act(heroName, health, attack, skills);

            hero.ChangeAttack(attackModified);

            hero.Attack.Should().Be(attackModified);
        }

        private static SkillHeroAssign CreateDefaultSkill()
        {
            return new SkillHeroAssign(Guid.NewGuid(), "skill#1", 100);
        }
    }
}
