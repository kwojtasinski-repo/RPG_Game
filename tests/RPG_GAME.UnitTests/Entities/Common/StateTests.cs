using FluentAssertions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Common;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Common
{
    public class StateTests
    {
        private State<T> Act<T>(T value, IncreasingState<T> increasingState) 
            where T : struct
            => new State<T>(value, increasingState);

        [Fact]
        public void should_create_state()
        {
            var value = 10;
            var increasingState = new IncreasingState<int>(1, "ADDITIVE");

            var state = Act<int>(value, increasingState);

            state.Should().NotBeNull();
            state.Value.Should().Be(value);
            state.IncreasingState.Value.Should().Be(increasingState.Value);
            state.IncreasingState.StrategyIncreasing.Should().Be(increasingState.StrategyIncreasing);
        }

        [Fact]
        public void given_null_increasing_state_should_throw_an_exception()
        {
            var expectedException = new InvalidIncreasingStateException();

            var exception = Record.Exception(() => Act<int>(10, null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_int_value_should_throw_an_exception()
        {
            var increasingState = new IncreasingState<int>(1, "ADDITIVE");
            var expectedException = new InvalidStateValueException();
            
            var exception = Record.Exception(() => Act<int>(-10, increasingState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
        
        [Fact]
        public void given_invalid_decimal_value_should_throw_an_exception()
        {
            var increasingState = new IncreasingState<decimal>(1M, "ADDITIVE");
            var expectedException = new InvalidStateValueException();
            
            var exception = Record.Exception(() => Act<decimal>(-10M, increasingState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_int_value_when_change_value_should_throw_an_exception()
        {
            var value = -100;
            var increasingState = new IncreasingState<int>(1, "ADDITIVE");
            var state = new State<int>(10, increasingState);
            var expectedException = new InvalidStateValueException();

            var exception = Record.Exception(() => state.ChangeValue(value));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }
        
        [Fact]
        public void given_invalid_decimal_value_when_change_value_should_throw_an_exception()
        {
            var value = -100M;
            var increasingState = new IncreasingState<decimal>(1, "ADDITIVE");
            var state = new State<decimal>(10, increasingState);
            var expectedException = new InvalidStateValueException();
            
            var exception = Record.Exception(() => state.ChangeValue(value));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_int_value()
        {
            var value = 100;
            var increasingState = new IncreasingState<int>(1, "ADDITIVE");
            var state = new State<int>(10, increasingState);

            state.ChangeValue(value);

            state.Value.Should().Be(value);
        }

        [Fact]
        public void should_change_decimal_value()
        {
            var value = 100;
            var increasingState = new IncreasingState<decimal>(1, "ADDITIVE");
            var state = new State<decimal>(10, increasingState);

            state.ChangeValue(value);

            state.Value.Should().Be(value);
        }

        [Fact]
        public void given_null_increasing_strategy_should_throw_an_exception()
        {
            var state = new State<decimal>(10, new IncreasingState<decimal>(1, "ADDITIVE"));
            var expectedException = new InvalidIncreasingStateException();

            var exception = Record.Exception(() => state.ChangeIncreasingState(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_increasing_state()
        {
            var increasingState = new IncreasingState<decimal>(12, "PERCENTAGE");
            var state = new State<decimal>(10, new IncreasingState<decimal>(1, "ADDITIVE"));

            state.ChangeIncreasingState(increasingState);

            state.IncreasingState.Value.Should().Be(increasingState.Value);
            state.IncreasingState.StrategyIncreasing.Should().Be(increasingState.StrategyIncreasing);
        }
    }
}
