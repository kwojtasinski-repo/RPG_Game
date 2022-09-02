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
        public void given_valid_map_should_create_battle()
        {
            var startDate = DateTime.UtcNow;
            var userId = Guid.NewGuid();
            var map = EntitiesFixture.CreateDefaultMap();

            var battle = Battle.Create(startDate, userId, map);

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

        [Fact]
        public void should_add_battle_state_at_prepare()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), "Starting", map);
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battleState = BattleState.Prepare(battle.Id, player, created);

            battle.AddBattleStateAtPrepare(battleState);

            battle.BattleStates.Should().NotBeEmpty();
            battle.BattleStates.Should().HaveCount(1);
            battle.BattleStates.Should().Contain(b => b.GetType() == battleState.GetType());
            battle.BattleStates.Should().Contain(b => b.Id == battleState.Id);
            battle.BattleInfo.Should().Be(BattleInfo.Starting);
        }

        [Fact]
        public void should_add_battle_state_at_in_progress()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(startDate, Guid.NewGuid(), map, player);
            var battleStateInAction = BattleState.InAction(battle.Id, player, created);

            battle.AddBattleStateAtInAction(battleStateInAction);

            battle.BattleStates.Should().NotBeEmpty();
            battle.BattleStates.Should().HaveCount(2);
            battle.BattleStates.Should().Contain(b => b.GetType() == battleStateInAction.GetType());
            battle.BattleStates.Should().Contain(b => b.Id == battleStateInAction.Id);
            battle.BattleInfo.Should().Be(BattleInfo.InProgress);
        }

        [Fact]
        public void should_end_battle()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var battleStateCompleted = BattleState.Completed(battle.Id, player, created);

            battle.EndBattle(created, "Suspended", battleStateCompleted);

            battle.BattleStates.Should().NotBeEmpty();
            battle.BattleStates.Should().HaveCount(3);
            battle.BattleStates.Should().Contain(b => b.GetType() == battleStateCompleted.GetType());
            battle.BattleStates.Should().Contain(b => b.Id == battleStateCompleted.Id);
            battle.BattleInfo.Should().Be(BattleInfo.Suspended);
        }

        [Fact]
        public void should_get_latest_battle_state_at_prepare()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(startDate, Guid.NewGuid(), map, player);

            var latestBattleState = battle.GetLatestBattleState();

            latestBattleState.Should().NotBeNull();
            latestBattleState.BattleStatus.Should().Be(BattleStatus.Prepare);
        }

        [Fact]
        public void should_get_latest_battle_state_at_in_action()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);

            var latestBattleState = battle.GetLatestBattleState();

            latestBattleState.Should().NotBeNull();
            latestBattleState.BattleStatus.Should().Be(BattleStatus.InAction);
        }

        [Fact]
        public void should_get_latest_battle_state_complete()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var battleStateCompleted = BattleState.Completed(battle.Id, player, created);
            battle.EndBattle(created, "Suspended", battleStateCompleted);

            var latestBattleState = battle.GetLatestBattleState();

            latestBattleState.Should().NotBeNull();
            latestBattleState.BattleStatus.Should().Be(BattleStatus.Completed);
        }

        [Fact]
        public void should_return_battle_state_in_action()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);

            var battleState = battle.GetBattleStateInAction();

            battleState.Should().NotBeNull();
            battleState.BattleStatus.Should().Be(BattleStatus.InAction);
        }
    }
}
