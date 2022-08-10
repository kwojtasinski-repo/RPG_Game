using FluentAssertions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Exceptions.Heroes;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Heroes
{
    public class SkillHeroTests
    {
        private SkillHero Act(string name, int baseAttack, IncreasingState<int> increasingState) => SkillHero.Create(name, baseAttack, increasingState);

        [Fact]
        public void should_create_skill_hero()
        {
            var name = "Skill#1";
            var baseAttack = 10;
            var increasingState = DefaultIncreasingState();

            var skill = Act(name, baseAttack, increasingState);

            skill.Should().NotBeNull();
            skill.BaseAttack.Should().Be(baseAttack);
        }

        [Fact]
        public void given_invalid_skill_name_should_throw_an_exception()
        {
            var name = "";
            var baseAttack = 102;
            var increasingState = DefaultIncreasingState();
            var expectedException = new InvalidSkillHeroNameException();

            var exception = Record.Exception(() => Act(name, baseAttack, increasingState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_increasing_state_should_throw_an_exception()
        {
            var name = "bat";
            var baseAttack = 100;
            var expectedException = new InvalidSkillHeroIncreasingStateException();

            var exception = Record.Exception(() => Act(name, baseAttack, null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_attack_should_throw_an_exception()
        {
            var name = "bat";
            var baseAttack = -20;
            var increasingState = DefaultIncreasingState();
            var expectedException = new InvalidSkillHeroAttackException();

            var exception = Record.Exception(() => Act(name, baseAttack, increasingState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_attack()
        {
            var attack = 25;
            var skillHero = SkillHero.Create("name", 20, DefaultIncreasingState());

            skillHero.ChangeSkillBaseAttack(attack);

            skillHero.BaseAttack.Should().Be(attack);
        }

        [Fact]
        public void given_invalid_attack_when_change_value_should_throw_an_exception()
        {
            var attack = -25;
            var skillHero = SkillHero.Create("name", 20, DefaultIncreasingState());
            var expectedException = new InvalidSkillHeroAttackException();

            var exception = Record.Exception(() => skillHero.ChangeSkillBaseAttack(attack));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_increasing_state_when_change_value_should_throw_an_exception()
        {
            var skillHero = SkillHero.Create("name", 10, DefaultIncreasingState());
            var expectedException = new InvalidSkillHeroIncreasingStateException();

            var exception = Record.Exception(() => skillHero.ChangeSkillIncreasingState(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_increasing_state()
        {
            var increasingState = new IncreasingState<int>(100, "PERCENTAGE");
            var skillHero = SkillHero.Create("name", 10, DefaultIncreasingState());

            skillHero.ChangeSkillIncreasingState(increasingState);

            skillHero.IncreasingState.Should().NotBeNull();
            skillHero.IncreasingState.StrategyIncreasing.Should().Be(increasingState.StrategyIncreasing);
            skillHero.IncreasingState.Value.Should().Be(increasingState.Value);
        }

        private static IncreasingState<int> DefaultIncreasingState()
        {
            return new IncreasingState<int>(1, "ADDITIVE");
        }
    }
}
