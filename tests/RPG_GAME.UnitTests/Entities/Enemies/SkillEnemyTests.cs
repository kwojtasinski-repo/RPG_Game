using FluentAssertions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Exceptions.Enemies;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Enemies
{
    public class SkillEnemyTests
    {
        private SkillEnemy Act(string name, int baseAttack, decimal probability, IncreasingState<int> increasingState) => SkillEnemy.Create(name, baseAttack, probability, increasingState);

        [Fact]
        public void should_create_skill_enemy()
        {
            var name = "Skill#1";
            var baseAttack = 10;
            var probability = 10M;
            var increasingState = DefaultIncreasingState();

            var skill = Act(name,baseAttack, probability, increasingState);

            skill.Should().NotBeNull();
            skill.BaseAttack.Should().Be(baseAttack);
            skill.Probability.Should().Be(probability);
        }

        [Fact]
        public void given_invalid_skill_name_should_throw_an_exception()
        {
            var name = "";
            var baseAttack = 10;
            var probability = 10M;
            var increasingState = DefaultIncreasingState();
            var expectedException = new InvalidSkillEnemyNameException();

            var exception = Record.Exception(() => Act(name, baseAttack, probability, increasingState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_increasing_state_should_throw_an_exception()
        {
            var name = "bat";
            var baseAttack = 10;
            var probability = 10M;
            var expectedException = new InvalidSkillEnemyIncreasingStateException();

            var exception = Record.Exception(() => Act(name, baseAttack, probability, null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(96)]
        [InlineData(100)]
        public void given_invalid_probability_should_throw_an_exception(decimal probability)
        {
            var name = "bat";
            var baseAttack = 10;
            var increasingState = DefaultIncreasingState();
            var expectedException = new InvalidSkillEnemyProbabilityException();

            var exception = Record.Exception(() => Act(name, baseAttack, probability, increasingState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_attack_should_throw_an_exception()
        {
            var name = "bat";
            var baseAttack = -20;
            var probability = 10M;
            var increasingState = DefaultIncreasingState();
            var expectedException = new InvalidSkillEnemyAttackException();

            var exception = Record.Exception(() => Act(name, baseAttack, probability, increasingState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private static IncreasingState<int> DefaultIncreasingState()
        {
            return new IncreasingState<int>(1, "ADDITIVE");
        }
    }
}
