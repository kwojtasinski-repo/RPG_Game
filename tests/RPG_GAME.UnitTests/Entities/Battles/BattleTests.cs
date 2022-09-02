using FluentAssertions;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Battles;
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

            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), BattleInfo.Starting.ToString(), map);

            battle.Should().NotBeNull();
            battle.StartDate.Should().Be(startDate);
            battle.Map.Id.Should().Be(map.Id);
            battle.Map.Name.Should().Be(map.Name);
        }

        [Theory]
        [InlineData("aqq")]
        [InlineData("124")]
        public void given_invalid_battle_info_when_creating_battle_should_throw_an_exception(string battleInfo)
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var expectedException = new InvalidBattleInfoException(battleInfo);

            var exception = Record.Exception(() => new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), battleInfo, map));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((InvalidBattleInfoException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
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
        public void given_null_map_when_create_battle_should_throw_an_exception()
        {
            var startDate = DateTime.UtcNow;
            var userId = Guid.NewGuid();
            var expectedException = new InvalidMapException();

            var exception = Record.Exception(() => Battle.Create(startDate, userId, null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
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
        public void given_map_with_no_enemies_when_get_enemy_to_fight_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), "Starting", map);
            var expectedException = new MapHasNoEnemiesException(map.Id);

            var exception = Record.Exception(() => battle.GetEnemyToFight());

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((MapHasNoEnemiesException)exception).MapId.Should().Be(expectedException.MapId);
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
            battle.AddBattleStateAtInAction(BattleState.InAction(battle.Id, EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), startDate));
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
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), BattleInfo.Starting.ToString(), map);
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
        public void given_existed_battle_state_prepare_when_add_battle_state_at_prepare_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), BattleInfo.Starting.ToString(), map);
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battleState = BattleState.Prepare(battle.Id, player, created);
            battle.AddBattleStateAtPrepare(battleState);
            var expectedException = new BattleStateAlreadyExistsException(battleState.BattleStatus);

            var exception = Record.Exception(() => battle.AddBattleStateAtPrepare(battleState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((BattleStateAlreadyExistsException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
        }

        [Theory]
        [MemberData(nameof(BattleStateDataForPrepare))]
        public void given_invalid_battle_state_when_add_battle_state_at_prepare_should_throw_an_exception(BattleState battleState)
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), BattleInfo.Starting.ToString(), map);
            var expectedException = new InvalidBattleStatusException(battleState.BattleStatus.ToString());

            var exception = Record.Exception(() => battle.AddBattleStateAtPrepare(battleState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((InvalidBattleStatusException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
        }

        [Fact]
        public void given_null_battle_state_when_add_battle_state_at_prepare_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), BattleInfo.Starting.ToString(), map);
            var expectedException = new InvalidBattleStateException();

            var exception = Record.Exception(() => battle.AddBattleStateAtPrepare(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_add_battle_state_at_in_action()
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
        public void given_existed_battle_state_in_action_when_add_battle_state_at_in_action_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(startDate, Guid.NewGuid(), map, player);
            var battleStateInAction = BattleState.InAction(battle.Id, player, created);
            battle.AddBattleStateAtInAction(battleStateInAction);
            var expectedException = new BattleStateAlreadyExistsException(battleStateInAction.BattleStatus);

            var exception = Record.Exception(() => battle.AddBattleStateAtInAction(battleStateInAction));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((BattleStateAlreadyExistsException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
        }

        [Fact]
        public void given_null_battle_state_when_add_battle_state_at_in_action_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(startDate, Guid.NewGuid(), map, player);
            var expectedException = new InvalidBattleStateException();

            var exception = Record.Exception(() => battle.AddBattleStateAtInAction(null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [MemberData(nameof(BattleStateDataForInAction))]
        public void given_invalid_battle_state_when_add_battle_state_at_in_actino_should_throw_an_exception(BattleState battleState)
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(startDate, Guid.NewGuid(), map, player);
            var expectedException = new InvalidBattleStatusException(battleState.BattleStatus.ToString());

            var exception = Record.Exception(() => battle.AddBattleStateAtInAction(battleState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((InvalidBattleStatusException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
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

            battle.EndBattle(created, BattleInfo.Suspended.ToString(), battleStateCompleted);

            battle.BattleStates.Should().NotBeEmpty();
            battle.BattleStates.Should().HaveCount(3);
            battle.BattleStates.Should().Contain(b => b.GetType() == battleStateCompleted.GetType());
            battle.BattleStates.Should().Contain(b => b.Id == battleStateCompleted.Id);
            battle.BattleInfo.Should().Be(BattleInfo.Suspended);
        }

        [Theory]
        [MemberData(nameof(BattleInfoData))]
        public void given_valid_battle_infos_without_won_when_end_battle_should_return_player_from_battle_state_at_action(BattleInfo battleInfo)
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var playerToUpdate = new Player(player.Id, player.Name, player.Hero, player.Level + 2, player.CurrentExp + 10, player.RequiredExp + 200, player.UserId);
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var battleStateCompleted = BattleState.Completed(battle.Id, playerToUpdate, created);

            var returnedPlayer = battle.EndBattle(created, battleInfo.ToString(), battleStateCompleted);

            returnedPlayer.Should().NotBeNull();
            returnedPlayer.Level.Should().Be(player.Level);
            returnedPlayer.CurrentExp.Should().Be(player.CurrentExp);
            returnedPlayer.RequiredExp.Should().Be(player.RequiredExp);
        }
        
        [Fact]
        public void given_battle_info_won_when_end_battle_should_return_player_from_battle_state_complete()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var playerToUpdate = new Player(player.Id, player.Name, player.Hero, player.Level + 2, player.CurrentExp + 10, player.RequiredExp + 200, player.UserId);
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var battleStateCompleted = BattleState.Completed(battle.Id, playerToUpdate, created);

            var returnedPlayer = battle.EndBattle(created, BattleInfo.Won.ToString(), battleStateCompleted);

            returnedPlayer.Should().NotBeNull();
            returnedPlayer.Level.Should().Be(playerToUpdate.Level);
            returnedPlayer.CurrentExp.Should().Be(playerToUpdate.CurrentExp);
            returnedPlayer.RequiredExp.Should().Be(playerToUpdate.RequiredExp);
        }

        [Fact]
        public void given_existing_battle_state_completed_when_end_battle_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var battleStateCompleted = BattleState.Completed(battle.Id, player, created);
            battle.EndBattle(created, BattleInfo.Suspended.ToString(), battleStateCompleted);
            var expectedException = new BattleStateAlreadyExistsException(battleStateCompleted.BattleStatus);

            var exception = Record.Exception(() => battle.EndBattle(created, BattleInfo.Suspended.ToString(), battleStateCompleted));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((BattleStateAlreadyExistsException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
        }

        [Fact]
        public void given_invalid_battle_info_when_end_battle_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var battleStateCompleted = BattleState.Completed(battle.Id, player, created);
            var battleInfo = "BattleInfo.Suspended.ToString()";
            var expectedException = new InvalidBattleInfoException(battleInfo);

            var exception = Record.Exception(() => battle.EndBattle(created, battleInfo, battleStateCompleted));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((InvalidBattleInfoException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
        }

        [Fact]
        public void given_null_battle_state_when_end_battle_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var expectedException = new InvalidBattleStateException();

            var exception = Record.Exception(() => battle.EndBattle(created, BattleInfo.Suspended.ToString(), null));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [MemberData(nameof(BattleStateDataForComplete))]
        public void given_invalid_battle_state_when_end_battle_should_throw_an_exception(BattleState battleState)
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var created = DateTime.UtcNow;
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var expectedException = new InvalidBattleStatusException(battleState.BattleStatus.ToString());

            var exception = Record.Exception(() => battle.EndBattle(created, BattleInfo.Suspended.ToString(), battleState));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((InvalidBattleStatusException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
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
            battle.EndBattle(created, BattleInfo.Suspended.ToString(), battleStateCompleted);

            var latestBattleState = battle.GetLatestBattleState();

            latestBattleState.Should().NotBeNull();
            latestBattleState.BattleStatus.Should().Be(BattleStatus.Completed);
        }

        [Fact]
        public void given_battle_with_empty_battle_states_when_get_latest_battle_state_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var battle = new Battle(Guid.NewGuid(), startDate, Guid.NewGuid(), BattleInfo.Starting.ToString(), map);
            var expectedException = new BattleStatesNotFoundException(battle.Id);

            var exception = Record.Exception(() => battle.GetLatestBattleState());
            
            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((BattleStatesNotFoundException)exception).BattleId.Should().Be(expectedException.BattleId);
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

        [Fact]
        public void given_invalid_battle_info_when_get_battle_state_in_action_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var map = EntitiesFixture.CreateDefaultMap();
            var hero = EntitiesFixture.CreateDefaultHero();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(startDate, Guid.NewGuid(), map, player);
            var expectedException = new CannotGetBattleStateInActionException(battle.BattleInfo);

            var exception = Record.Exception(() => battle.GetBattleStateInAction());

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotGetBattleStateInActionException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
        }

        [Fact]
        public void should_add_killed_enemy()
        {
            var startDate = DateTime.Now;
            var hero = EntitiesFixture.CreateDefaultHero();
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Core.Entities.Maps.Enemies> { new Core.Entities.Maps.Enemies(enemy, 1) });
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);

            battle.AddKilledEnemy(enemy.Id);

            battle.EnemiesKilled.Should().NotBeEmpty();
            battle.EnemiesKilled.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public void given_invalid_battle_info_when_add_killed_enemy_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var hero = EntitiesFixture.CreateDefaultHero();
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Core.Entities.Maps.Enemies> { new Core.Entities.Maps.Enemies(enemy, 1) });
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleAtPrepare(startDate, Guid.NewGuid(), map, player);
            var expectedException = new CannotAddKilledEnemyToBattleWithBattleInfoException(battle.BattleInfo);

            var exception = Record.Exception(() => battle.AddKilledEnemy(enemy.Id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotAddKilledEnemyToBattleWithBattleInfoException)exception).BattleInfo.Should().Be(expectedException.BattleInfo);
        }

        [Fact]
        public void given_exceeded_enemies_when_add_killed_enemy_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var hero = EntitiesFixture.CreateDefaultHero();
            var enemy = EntitiesFixture.CreateDefaultEnemy().AsAssign();
            var map = EntitiesFixture.CreateDefaultMap(enemies: new List<Core.Entities.Maps.Enemies> { new Core.Entities.Maps.Enemies(enemy, 1) });
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            battle.AddKilledEnemy(enemy.Id);
            var expectedException = new EnemiesKilledQuantityHasBeenExceededException(enemy.Id);

            var exception = Record.Exception(() => battle.AddKilledEnemy(enemy.Id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((EnemiesKilledQuantityHasBeenExceededException)exception).EnemyId.Should().Be(expectedException.EnemyId);
        }

        [Fact]
        public void given_invalid_enemy_id_when_add_killed_enemy_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var hero = EntitiesFixture.CreateDefaultHero();
            var map = EntitiesFixture.CreateDefaultMap();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var enemyId = Guid.NewGuid();
            var expectedException = new CannotAddEnemyOutsideMapException(enemyId);

            var exception = Record.Exception(() => battle.AddKilledEnemy(enemyId));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotAddEnemyOutsideMapException)exception).EnemyId.Should().Be(expectedException.EnemyId);
        }

        [Fact]
        public void given_empty_enemy_id_when_add_killed_enemy_should_throw_an_exception()
        {
            var startDate = DateTime.Now;
            var hero = EntitiesFixture.CreateDefaultHero();
            var map = EntitiesFixture.CreateDefaultMap();
            var player = EntitiesFixture.CreateDefaultPlayer(hero, Guid.NewGuid());
            var battle = BattleFixture.CreateBattleInProgress(startDate, Guid.NewGuid(), map, player);
            var expectedException = new InvalidEnemyIdException();

            var exception = Record.Exception(() => battle.AddKilledEnemy(Guid.Empty));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        public static TheoryData<BattleInfo> BattleInfoData
        {
            get
            {
                return new()
                {
                    BattleInfo.Lost,
                    BattleInfo.Suspended
                };
            }
        }

        public static TheoryData<BattleState> BattleStateDataForPrepare
        {
            get
            {
                return new()
                {
                    BattleState.InAction(Guid.NewGuid(), EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), DateTime.UtcNow),
                    BattleState.Completed(Guid.NewGuid(), EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), DateTime.UtcNow)
                };
            }
        }

        public static TheoryData<BattleState> BattleStateDataForInAction
        {
            get
            {
                return new()
                {
                    BattleState.Prepare(Guid.NewGuid(), EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), DateTime.UtcNow),
                    BattleState.Completed(Guid.NewGuid(), EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), DateTime.UtcNow)
                };
            }
        }

        public static TheoryData<BattleState> BattleStateDataForComplete
        {
            get
            {
                return new()
                {
                    BattleState.Prepare(Guid.NewGuid(), EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), DateTime.UtcNow),
                    BattleState.InAction(Guid.NewGuid(), EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), DateTime.UtcNow)
                };
            }
        }
    }
}
