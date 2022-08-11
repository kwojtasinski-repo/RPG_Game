using FluentAssertions;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Players;
using System;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Players
{
    public class PlayerTests
    {
        private Player Act(string name, HeroAssign hero, int level, decimal currentExp, decimal requiredExp, Guid userId)
            => new(Guid.NewGuid(), name, hero, level, currentExp, requiredExp, userId);

        [Fact]
        public void should_create()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();

            var player = Player.Create(name, hero,requiredExp, userId);

            player.Should().NotBeNull();
            player.Name.Should().Be(name);
            player.Level.Should().Be(level);
            player.CurrentExp.Should().Be(currentExp);
            player.RequiredExp.Should().Be(requiredExp);
            player.UserId.Should().Be(userId);
        }

        [Fact]
        public void given_empty_name_should_throw_an_exception()
        {
            var name = "";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var expectedException = new InvalidPlayerNameException();

            var exception = Record.Exception(() => Act(name, hero, level, currentExp, requiredExp, userId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_too_short_name_should_throw_an_exception()
        {
            var name = "P";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var expectedException = new TooShortPlayerNameException(name);

            var exception = Record.Exception(() => Act(name, hero, level, currentExp, requiredExp, userId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-50)]
        public void given_invalid_level_should_throw_an_exception(int level)
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var expectedException = new PlayerLevelCannotZeroOrNegativeException(level);

            var exception = Record.Exception(() => Act(name, hero, level, currentExp, requiredExp, userId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_current_exp_should_throw_an_exception()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var requiredExp = 100;
            var level = 1;
            var userId = Guid.NewGuid();
            var currentExp = -1;
            var expectedException = new PlayerCurrentExpCannotBeNegativeException(currentExp);
            
            var exception = Record.Exception(() => Act(name, hero, level, currentExp, requiredExp, userId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-20)]
        public void given_invalid_required_exp_should_throw_an_exception(decimal requiredExp)
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var currentExp = 100;
            var level = 1;
            var userId = Guid.NewGuid();
            var expectedException = new PlayerRequiredExpCannotBeZeroOrNegativeException(requiredExp);

            var exception = Record.Exception(() => Act(name, hero, level, currentExp, requiredExp, userId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_name()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var nameModified = "Name#1";

            player.ChangeName(nameModified);

            player.Name.Should().Be(nameModified);
        }

        [Fact]
        public void given_empty_name_should_when_change_value_throw_an_exception()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var nameModified = "";
            var expectedException = new InvalidPlayerNameException();

            var exception = Record.Exception(() => player.ChangeName(nameModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_too_short_name_when_change_value_should_throw_an_exception()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var nameModified = "Na";
            var expectedException = new TooShortPlayerNameException(nameModified);

            var exception = Record.Exception(() => player.ChangeName(nameModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        public void should_increase_current_exp(decimal increaseExpBy)
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var totalCurrentExp = currentExp + increaseExpBy;

            player.IncreaseCurrentExpBy(increaseExpBy);

            player.CurrentExp.Should().Be(totalCurrentExp);
        }

        [Fact]
        public void given_invalid_increase_current_exp_when_change_value_should_throw_an_exception()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var currentExpModified = -1;
            var expectedException = new PlayerCurrentExpCannotBeNegativeException(currentExpModified);

            var exception = Record.Exception(() => player.IncreaseCurrentExpBy(currentExpModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(30)]
        [InlineData(40)]
        public void should_increase_current_exp_and_level_up(decimal increaseExpBy)
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 70;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var expAfterLevelUp = (currentExp + increaseExpBy) - requiredExp;

            player.IncreaseCurrentExpBy(increaseExpBy);

            player.Level.Should().Be(2);
            player.CurrentExp.Should().Be(expAfterLevelUp);
        }

        [Fact]
        public void should_change_required_exp()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var reqExp = 2000;

            player.ChangeRequiredExp(reqExp);

            player.RequiredExp.Should().Be(reqExp);
        }

        [Fact]
        public void given_invalid_required_exp_when_change_value_should_throw_an_exception()
        {
            var name = "Player#1";
            var hero = DefaultHeroAssign();
            var level = 1;
            var currentExp = 0;
            var requiredExp = 100;
            var userId = Guid.NewGuid();
            var player = Act(name, hero, level, currentExp, requiredExp, userId);
            var reqExp = -2000;
            var expectedException = new PlayerRequiredExpCannotBeZeroOrNegativeException(reqExp);

            var exception = Record.Exception(() => player.ChangeRequiredExp(reqExp));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private static HeroAssign DefaultHeroAssign()
        {
            return new HeroAssign(Guid.NewGuid(), "Hero", 100, 20, 5);
        }
    }
}
