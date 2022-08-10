using FluentAssertions;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Exceptions.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Maps
{
    public class MapTests
    {
        private Map Act(string name, string difficulty, IEnumerable<RPG_GAME.Core.Entities.Maps.Enemies> enemies = null) => Map.Create(name, difficulty, enemies);

        [Fact]
        public void should_create_map()
        {
            var name = "Map#1";
            var difficulty = "EASY";

            var map = Act(name, difficulty);

            map.Should().NotBeNull();
            map.Name.Should().Be(name);
            map.Difficulty.ToString().Should().Be(difficulty);
            map.Enemies.Should().BeEmpty();
        }

        [Fact]
        public void should_create_map_with_enemies()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var enemies = new List<Core.Entities.Maps.Enemies> { CreateDefaultEnemies() };

            var map = Act(name, difficulty, enemies);

            map.Should().NotBeNull();
            map.Name.Should().Be(name);
            map.Difficulty.ToString().Should().Be(difficulty);
            map.Enemies.Should().NotBeEmpty();
            map.Enemies.Should().HaveCount(enemies.Count);
        }

        [Fact]
        public void should_change_name()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var map = Map.Create("mapp", difficulty);

            map.ChangeName(name);

            map.Name.Should().Be(name);
        }

        [Fact]
        public void given_invalid_name_should_throw_an_exception()
        {
            var difficulty = "EASY";
            var map = Map.Create("mapp", difficulty);
            var expectedException = new InvalidMapNameException();

            var exception = Record.Exception(() => map.ChangeName(""));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_too_short_name_should_throw_an_exception()
        {
            var name = "Ma";
            var difficulty = "EASY";
            var map = Map.Create("mapp", difficulty);
            var expectedException = new TooShortMapNameException(name);

            var exception = Record.Exception(() => map.ChangeName(name));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_difficulty()
        {
            var name = "Map#1";
            var difficulty = "MEDIUM";
            var map = Map.Create(name, "EASY");

            map.ChangeDifficulty(difficulty);

            map.Difficulty.ToString().Should().Be(difficulty);
        }

        [Fact]
        public void given_invalid_difficulty_should_throw_an_exception()
        {
            var name = "Map#1";
            var map = Map.Create(name, "EASY");
            var invalidDifficulty = "abc";
            var expectedException = new InvalidMapDifficultyException(invalidDifficulty);

            var exception = Record.Exception(() => map.ChangeDifficulty(invalidDifficulty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_add_enemies()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var map = Map.Create(name, difficulty);

            map.AddEnemies(CreateDefaultEnemies());

            map.Enemies.Should().NotBeEmpty();
            map.Enemies.Should().HaveCount(1);
        }

        [Fact]
        public void should_add_quantity_to_existed_enemies()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var enemy = CreateDefaultEnemies();
            var quantityExpected = enemy.Quantity + enemy.Quantity;
            var map = Map.Create(name, difficulty);
            map.AddEnemies(enemy);

            map.AddEnemies(enemy);

            map.Enemies.Should().NotBeEmpty();
            map.Enemies.Should().HaveCount(1);
            map.Enemies.First().Quantity.Should().Be(quantityExpected);
        }

        [Fact]
        public void given_null_enemies_when_add_should_throw_an_exception()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var map = Map.Create(name, difficulty);
            var expectedException = new InvalidEnemiesException();

            var exception = Record.Exception(() => map.AddEnemies(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_delete_enemies()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var enemy = CreateDefaultEnemies();
            var enemies = new List<Core.Entities.Maps.Enemies> { enemy };
            var map = Map.Create(name, difficulty, enemies);

            map.RemoveEnemy(enemy);

            map.Enemies.Should().BeEmpty();
            map.Enemies.Should().HaveCount(0);
        }

        [Fact]
        public void should_delete_only_one_enemy()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var enemy = CreateDefaultEnemies();
            enemy.AddQuantity(20);
            var enemyToRemove = CreateDefaultEnemies(enemy.Enemy.Id);
            var quantity = enemy.Quantity;
            var enemies = new List<Core.Entities.Maps.Enemies> { enemy };
            var map = Map.Create(name, difficulty, enemies);

            map.RemoveEnemy(enemyToRemove);

            map.Enemies.Should().NotBeEmpty();
            map.Enemies.Should().HaveCount(1);
            map.Enemies.First().Quantity.Should().Be(quantity - enemyToRemove.Quantity);
        }

        [Fact]
        public void given_not_existed_enemies_should_throw_an_exception()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var enemy = CreateDefaultEnemies();
            var map = Map.Create(name, difficulty);
            var expectedException = new EnemiesDoesntExistsException(enemy.Enemy.Id);

            var exception = Record.Exception(() => map.RemoveEnemy(enemy));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_enemies_should_throw_an_exception()
        {
            var name = "Map#1";
            var difficulty = "EASY";
            var map = Map.Create(name, difficulty);
            var expectedException = new InvalidEnemiesException();

            var exception = Record.Exception(() => map.RemoveEnemy(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        private static RPG_GAME.Core.Entities.Maps.Enemies CreateDefaultEnemies(Guid? id = null)
        {
            return new Core.Entities.Maps.Enemies(
                new EnemyAssign(id ?? Guid.NewGuid(), "Enemy#1", 10, 10, 1, 100, "EASY"),
                10);
        }
    }
}
