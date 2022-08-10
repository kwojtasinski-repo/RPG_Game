using FluentAssertions;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Exceptions.Maps;
using System;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Maps
{
    public class SkillEnemyAssignTests
    {
        private SkillEnemyAssign Act(string name, int attack, decimal probability) => new SkillEnemyAssign(Guid.NewGuid(), name, attack, probability);

        [Fact]
        public void should_create()
        {
            var name = "Skill#1";
            var attack = 200;
            var probability = 30;

            var skill = Act(name, attack, probability);

            skill.Should().NotBeNull();
            skill.Name.Should().Be(name);
            skill.Attack.Should().Be(attack);
            skill.Probability.Should().Be(probability);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-20)]
        public void given_invalid_attack_should_throw_an_exception(int attack)
        {
            var name = "Skill#1";
            var probability = 30;
            var expectedException = new AttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => Act(name, attack, probability));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-20)]
        public void given_invalid_attack_when_change_value_should_throw_an_exception(int attack)
        {
            var name = "Skill#1";
            var probability = 30;
            var skill = Act(name, 100, probability);
            var expectedException = new AttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => skill.ChangeAttack(attack));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
    }
}
