using FluentAssertions;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Exceptions.Maps;
using System;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Maps
{
    public class EnemyAssignTests
    {
        private EnemyAssign Act(string enemyName, int attack, int health, int heal, decimal experience, string difficulty, IEnumerable<SkillEnemyAssign> skills = null)
            => new EnemyAssign(Guid.NewGuid(), enemyName, attack, health, heal, experience, difficulty, skills);

        [Fact]
        public void should_create()
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";

            var enemy = Act(name, attack, health, heal, experience, difficulty);

            enemy.Should().NotBeNull();
            enemy.EnemyName.Should().Be(name);
            enemy.Attack.Should().Be(attack);
            enemy.Health.Should().Be(health);
            enemy.HealLvl.Should().Be(heal);
            enemy.Experience.Should().Be(experience);
            enemy.Difficulty.ToString().Should().Be(difficulty);
            enemy.Skills.Should().BeEmpty();
        }

        [Fact]
        public void should_create_with_skills()
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var skills = new List<SkillEnemyAssign> { new SkillEnemyAssign(Guid.NewGuid(), "skill#1", 350, 20) };

            var enemy = Act(name, attack, health, heal, experience, difficulty, skills);

            enemy.Should().NotBeNull();
            enemy.EnemyName.Should().Be(name);
            enemy.Attack.Should().Be(attack);
            enemy.Health.Should().Be(health);
            enemy.HealLvl.Should().Be(heal);
            enemy.Experience.Should().Be(experience);
            enemy.Difficulty.ToString().Should().Be(difficulty);
            enemy.Skills.Should().NotBeEmpty();
            enemy.Skills.Should().HaveCount(skills.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-501)]
        public void given_invalid_attack_should_throw_an_exception(int attack)
        {
            var name = "Enemy#1";
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var expectedException = new AttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => Act(name, attack, health, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-21)]
        public void given_invalid_health_should_throw_an_exception(int health)
        {
            var name = "Enemy#1";
            var attack = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var expectedException = new HealthCannotBeZeroOrNegativeException(health);

            var exception = Record.Exception(() => Act(name, attack, health, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-31)]
        public void given_invalid_heal_should_throw_an_exception(int heal)
        {
            var name = "Enemy#1";
            var health = 150;
            var attack = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var expectedException = new HealLvlCannotBeZeroOrNegativeException(heal);

            var exception = Record.Exception(() => Act(name, attack, health, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-41)]
        public void given_invalid_experience_should_throw_an_exception(decimal experience)
        {
            var name = "Enemy#1";
            var health = 150;
            var heal = 10;
            var attack = 1000;
            var difficulty = "HARD";
            var expectedException = new ExperienceCannotBeZeroOrNegativeException(experience);

            var exception = Record.Exception(() => Act(name, attack, health, heal, experience, difficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_attack()
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var atackModified = 1000;

            enemy.ChangeAttack(atackModified);

            enemy.Attack.Should().Be(atackModified);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-501)]
        public void given_invalid_attack_when_modified_value_should_throw_an_exception(int atackModified)
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var expectedException = new AttackCannotBeZeroOrNegativeException(atackModified);

            var exception = Record.Exception(() => enemy.ChangeAttack(atackModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
        
        [Fact]
        public void should_change_health()
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var healthModified = 20000;

            enemy.ChangeHealth(healthModified);

            enemy.Health.Should().Be(healthModified);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-21)]
        public void given_invalid_health_when_change_value_should_throw_an_exception(int healthModified)
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var expectedException = new HealthCannotBeZeroOrNegativeException(healthModified);

            var exception = Record.Exception(() => enemy.ChangeHealth(healthModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
        
        [Fact]
        public void should_change_heal()
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var healModified = 200;

            enemy.ChangeHealLvl(healModified);

            enemy.HealLvl.Should().Be(healModified);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-31)]
        public void given_invalid_heal_when_change_value_should_throw_an_exception(int healModified)
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var expectedException = new HealLvlCannotBeZeroOrNegativeException(healModified);

            var exception = Record.Exception(() => enemy.ChangeHealLvl(healModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
        
        [Fact]
        public void should_change_experience()
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var experienceModified = 2500M;

            enemy.ChangeExperience(experienceModified);

            enemy.Experience.Should().Be(experienceModified);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-41)]
        public void given_invalid_experience_when_change_value_should_throw_an_exception(decimal experienceModified)
        {
            var name = "Enemy#1";
            var attack = 200;
            var health = 150;
            var heal = 10;
            var experience = 1000M;
            var difficulty = "HARD";
            var enemy = Act(name, attack, health, heal, experience, difficulty);
            var expectedException = new ExperienceCannotBeZeroOrNegativeException(experienceModified);

            var exception = Record.Exception(() => enemy.ChangeExperience(experienceModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
    }
}
