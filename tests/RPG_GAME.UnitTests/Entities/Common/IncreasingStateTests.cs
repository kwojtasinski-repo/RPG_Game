using FluentAssertions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Common;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Common
{
    public class IncreasingStateTests
    {
        private IncreasingState<T> Act<T>(T value, string strategyIncreasing)
            where T : struct
            => new IncreasingState<T>(value, strategyIncreasing);

        [Fact]
        public void should_create()
        {
            var value = 10;

            var increasingState = Act<int>(value, "ADDITIVE");

            increasingState.Should().NotBeNull();
            increasingState.Value.Should().Be(value);
            increasingState.StrategyIncreasing.Should().Be(increasingState.StrategyIncreasing);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("atest")]
        public void given_null_increasing_state_should_throw_an_exception(string strategyIncreasing)
        {
            var expectedException = new InvalidStrategyIncreasingException(strategyIncreasing);

            var exception = Record.Exception(() => Act<int>(10, strategyIncreasing));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_int_value_should_throw_an_exception()
        {
            var expectedException = new InvalidStateValueException();

            var exception = Record.Exception(() => Act<int>(-11, "ADDITIVE"));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_decimal_value_should_throw_an_exception()
        {
            var expectedException = new InvalidStateValueException();

            var exception = Record.Exception(() => Act<decimal>(-11M, "ADDITIVE"));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_int_value_when_change_value_should_throw_an_exception()
        {
            var value = -100;
            var increasingState = new IncreasingState<int>(1, "ADDITIVE");
            var expectedException = new InvalidStateValueException();

            var exception = Record.Exception(() => increasingState.ChangeValue(value));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_decimal_value_when_change_value_should_throw_an_exception()
        {
            var value = -100M;
            var increasingState = new IncreasingState<decimal>(1, "ADDITIVE");
            var expectedException = new InvalidStateValueException();

            var exception = Record.Exception(() => increasingState.ChangeValue(value));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_int_value()
        {
            var value = 100;
            var increasingState = new IncreasingState<int>(1, "ADDITIVE");

            increasingState.ChangeValue(value);

            increasingState.Value.Should().Be(value);
        }

        [Fact]
        public void should_change_decimal_value()
        {
            var value = 100;
            var increasingState = new IncreasingState<decimal>(1, "ADDITIVE");

            increasingState.ChangeValue(value);

            increasingState.Value.Should().Be(value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        public void given_invalid_increasing_strategy_should_throw_an_exception(string increasingStrategy)
        {
            var increasingState = new IncreasingState<decimal>(1, "ADDITIVE");
            var expectedException = new InvalidStrategyIncreasingException(increasingStrategy);

            var exception = Record.Exception(() => increasingState.ChangeStrategyIncreasing(increasingStrategy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_increasing_state()
        {
            var increasingState = new IncreasingState<decimal>(12, "ADDITIVE");
            var strategy = "PERCENTAGE";

            increasingState.ChangeStrategyIncreasing(strategy);

            increasingState.StrategyIncreasing.Should().Be(increasingState.StrategyIncreasing);
        }
    }
}
