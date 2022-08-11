using FluentAssertions;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Exceptions.Maps;
using System;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Maps
{
    public class EnemiesTests
    {
        private RPG_GAME.Core.Entities.Maps.Enemies Act(EnemyAssign enemy, int quantity)
            => new RPG_GAME.Core.Entities.Maps.Enemies(enemy, quantity);

        [Fact]
        public void should_create_enemies()
        {
            var enemy = DefaultEnemyAssing();
            var quantity = 10;

            var enemies = Act(enemy, quantity);

            enemies.Should().NotBeNull();
            enemies.Enemy.Id.Should().Be(enemy.Id);
            enemies.Quantity.Should().Be(quantity);
        }

        [Fact]
        public void given_null_enemy_assign_should_throw_an_exception()
        {
            var expectedException = new InvalidEnemyException();
            var quantity = 10;

            var exception = Record.Exception(() => Act(null, quantity));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1000)]
        public void given_negative_quantity_should_throw_an_exception(int quantity)
        {
            var enemy = DefaultEnemyAssing();
            var expectedException = new QuantityCannotBeZeroOrNegative(quantity);

            var exception = Record.Exception(() => Act(enemy, quantity));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_quantity_should_add()
        {
            var enemy = DefaultEnemyAssing();
            var currentQuantity = 10;
            var enemies = Act(enemy, currentQuantity);
            var quantity = 1;
            var totalQuantity = currentQuantity + quantity;

            enemies.AddQuantity(quantity);

            enemies.Quantity.Should().Be(totalQuantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-200)]
        public void given_invalid_quantity_when_add_value_should_throw_an_exception(int quantity)
        {
            var expectedException = new QuantityCannotBeZeroOrNegative(quantity);
            var enemy = DefaultEnemyAssing();
            var currentQuantity = 10;
            var enemies = Act(enemy, currentQuantity);

            var exception = Record.Exception(() => enemies.AddQuantity(quantity));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_valid_quantity_should_remove()
        {
            var enemy = DefaultEnemyAssing();
            var currentQuantity = 10;
            var enemies = Act(enemy, currentQuantity);
            var quantity = 2;
            var totalQuantity = currentQuantity - quantity;

            enemies.RemoveQuantity(quantity);

            enemies.Quantity.Should().Be(totalQuantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-50)]
        public void given_invalid_quantity_when_remove_value_should_throw_an_exception(int quantity)
        {
            var enemy = DefaultEnemyAssing();
            var currentQuantity = 1;
            var enemies = Act(enemy, currentQuantity);
            var expectedException = new QuantityCannotBeZeroOrNegative(quantity);

            var exception = Record.Exception(() => enemies.RemoveQuantity(quantity));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(200)]
        public void total_quantity_when_delete_is_zero_or_negative_should_throw_an_exception(int quantity)
        {
            var enemy = DefaultEnemyAssing();
            var currentQuantity = 1;
            var enemies = Act(enemy, currentQuantity);
            var quantitySubtract = currentQuantity - quantity;
            var expectedException = new QuantityCannotBeZeroOrNegative(quantitySubtract);

            var exception = Record.Exception(() => enemies.RemoveQuantity(quantity));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_replace_quantity()
        {
            var enemy = DefaultEnemyAssing();
            var enemies = new Core.Entities.Maps.Enemies(enemy, 1);
            var quantity = 15;

            enemies.SetQuantity(quantity);

            enemies.Quantity.Should().Be(quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void given_invalid_quantiy_when_replace_should_throw_an_exception(int quantity)
        {
            var enemy = DefaultEnemyAssing();
            var enemies = new Core.Entities.Maps.Enemies(enemy, 1);
            var expectedException = new QuantityCannotBeZeroOrNegative(quantity);

            var exception = Record.Exception(() => enemies.SetQuantity(quantity));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private static EnemyAssign DefaultEnemyAssing()
        {
            return new EnemyAssign(Guid.NewGuid(), "Enemy", 10, 200, 10, 2000, "EASY");
        }
    }
}
