using FluentAssertions;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Players;
using System;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Players
{
    public class SkillHeroAssignTests
    {
        private SkillHeroAssign Act(string name, int attack)
            => new SkillHeroAssign(Guid.NewGuid(), name, attack);

        [Fact]
        public void should_create()
        {
            var name = "skill";
            var attack = 10;

            var skill = Act(name, attack);

            skill.Should().NotBeNull();
            skill.Name.Should().Be(name);
            skill.Attack.Should().Be(attack);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public void given_invalid_attack_should_throw_an_exception(int attack)
        {
            var name = "skill";
            var expectedException = new PlayerSkillAttackCannotBeZeroOrNegativeException(attack);

            var exception = Record.Exception(() => Act(name, attack));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-5)]
        public void given_invalid_attack_when_change_value_should_throw_an_exception(int attackModified)
        {
            var name = "skill";
            var attack = 10;
            var skill = Act(name, attack);
            var expectedException = new PlayerSkillAttackCannotBeZeroOrNegativeException(attackModified);

            var exception = Record.Exception(() => skill.ChangeAttack(attackModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
    }
}
