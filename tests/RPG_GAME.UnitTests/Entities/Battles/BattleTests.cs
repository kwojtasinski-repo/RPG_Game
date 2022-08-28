using FluentAssertions;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Battles
{
    public class BattleTests
    {
        [Fact]
        public void should_create_battle()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();

            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), "Starting", map);

            battle.Should().NotBeNull();
            battle.StartDate.Should().Be(startDate);
            battle.Map.Id.Should().Be(map.Id);
            battle.Map.Name.Should().Be(map.Name);
        }

        [Fact]
        public void should_return_enemy_to_fight()
        {
            var startDate = DateTime.Now;
            var enemy1 = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var enemy2 = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var enemy3 = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Core.Entities.Maps.Enemies> { new Core.Entities.Maps.Enemies(enemy1, 1), new Core.Entities.Maps.Enemies(enemy2, 2), new Core.Entities.Maps.Enemies(enemy3, 3) });
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), "Starting", map);

            var enemyToFight = battle.GetEnemyToFight();

            enemyToFight.Should().NotBeNull();
            enemyToFight.Id.Should().Be(enemy1.Id);
        }

        [Fact]
        public void should_return_enemy_to_fight_when_one_is_killed()
        {
            var startDate = DateTime.Now;
            var enemy1 = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var enemy2 = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var enemy3 = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Core.Entities.Maps.Enemies> { new Core.Entities.Maps.Enemies(enemy1, 1), new Core.Entities.Maps.Enemies(enemy2, 2), new Core.Entities.Maps.Enemies(enemy3, 3) });
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), "Starting", map);
            battle.AddKilledEnemy(enemy1.Id);

            var enemyToFight = battle.GetEnemyToFight();

            enemyToFight.Should().NotBeNull();
            enemyToFight.Id.Should().Be(enemy2.Id);
        }
    }
}
