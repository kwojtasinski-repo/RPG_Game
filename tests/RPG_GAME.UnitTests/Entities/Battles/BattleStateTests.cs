using FluentAssertions;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Battles;
using RPG_GAME.UnitTests.Fixtures;
using System;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Battles
{
    public class BattleStateTests
    {
        [Fact]
        public void should_create_battle_state_at_prepare()
        {
            var battleId = Guid.NewGuid();
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var created = DateTime.UtcNow;

            var battleState = BattleState.Prepare(battleId, player, created);

            battleState.Should().NotBeNull();
            battleState.BattleId.Should().Be(battleId);
            battleState.Player.Id.Should().Be(player.Id);
            battleState.Created.Should().Be(created);
            battleState.BattleStatus.Should().Be(BattleStatus.Prepare);
        }

        [Fact]
        public void should_create_battle_state_at_in_action()
        {
            var battleId = Guid.NewGuid();
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var created = DateTime.UtcNow;

            var battleState = BattleState.InAction(battleId, player, created);

            battleState.Should().NotBeNull();
            battleState.BattleId.Should().Be(battleId);
            battleState.Player.Id.Should().Be(player.Id);
            battleState.Created.Should().Be(created);
            battleState.BattleStatus.Should().Be(BattleStatus.InAction);
        }

        [Fact]
        public void should_create_battle_state_at_complete()
        {
            var battleId = Guid.NewGuid();
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var created = DateTime.UtcNow;

            var battleState = BattleState.Completed(battleId, player, created);

            battleState.Should().NotBeNull();
            battleState.BattleId.Should().Be(battleId);
            battleState.Player.Id.Should().Be(player.Id);
            battleState.Created.Should().Be(created);
            battleState.BattleStatus.Should().Be(BattleStatus.Completed);
        }

        [Theory]
        [InlineData("status")]
        [InlineData("123")]
        public void given_invalid_battle_status_should_throw_an_exception(string battleStatus)
        {
            var expectedException = new InvalidBattleStatusException(battleStatus);

            var exception = Record.Exception(() => new BattleState(Guid.NewGuid(), battleStatus, Guid.NewGuid(), 
                EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid()), DateTime.UtcNow));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((InvalidBattleStatusException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
        }

        [Fact]
        public void should_update_player()
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var battleState = BattleState.InAction(Guid.NewGuid(), player, DateTime.UtcNow);
            var playerToUpdate = new Player(player.Id, player.Name, player.Hero, player.Level + 10, player.CurrentExp, player.RequiredExp + 1000, Guid.NewGuid());

            battleState.UpdatePlayer(playerToUpdate, DateTime.UtcNow);

            battleState.Player.Should().NotBeNull();
            battleState.Player.Level.Should().Be(playerToUpdate.Level);
            battleState.Player.RequiredExp.Should().Be(playerToUpdate.RequiredExp);
            battleState.Modified.Should().NotBeNull();
        }

        [Fact]
        public void given_invalid_modified_date_should_throw_an_exception()
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var battleState = BattleState.InAction(Guid.NewGuid(), player, DateTime.UtcNow);
            var modifiedDate = DateTime.UtcNow;
            var playerToUpdate = new Player(player.Id, player.Name, player.Hero, player.Level + 10, player.CurrentExp, player.RequiredExp + 1000, Guid.NewGuid());
            var nextModifiedDate = new DateTime(2019, 7, 24);
            battleState.UpdatePlayer(playerToUpdate, modifiedDate);
            var expectedException = new ModifiedDateCannotBeBeforeException(modifiedDate, nextModifiedDate);

            var exception = Record.Exception(() => battleState.UpdatePlayer(playerToUpdate, nextModifiedDate));
            
            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((ModifiedDateCannotBeBeforeException)exception).CurrentModifiedDate.Should().Be(modifiedDate);
            ((ModifiedDateCannotBeBeforeException)exception).NextModifiedDate.Should().Be(nextModifiedDate);
        }

        [Fact]
        public void given_default_modified_date_should_throw_an_exception()
        {
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var battleState = BattleState.InAction(Guid.NewGuid(), player, DateTime.UtcNow);
            var playerToUpdate = new Player(player.Id, player.Name, player.Hero, player.Level + 10, player.CurrentExp, player.RequiredExp + 1000, Guid.NewGuid());
            var nextModifiedDate = new DateTime(); // default DateTime
            var expectedException = new InvalidModifiedDateException();

            var exception = Record.Exception(() => battleState.UpdatePlayer(playerToUpdate, nextModifiedDate));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Theory]
        [MemberData(nameof(BattleStateData))]
        public void given_invalid_battle_status_when_update_player_should_throw_an_exception(BattleState battleState)
        {
            var playerToUpdate = new Player(battleState.Player.Id, battleState.Player.Name, battleState.Player.Hero, battleState.Player.Level + 10, battleState.Player.CurrentExp, battleState.Player.RequiredExp + 1000, Guid.NewGuid());
            var expectedException = new CannotUpdatePlayerInCurrentBattleStateException(battleState.BattleStatus);

            var exception = Record.Exception(() => battleState.UpdatePlayer(playerToUpdate, DateTime.UtcNow));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((CannotUpdatePlayerInCurrentBattleStateException)exception).BattleStatus.Should().Be(expectedException.BattleStatus);
        }


        [Fact]
        public void given_null_player_when_update_player_should_throw_an_exception()
        {
            var battleId = Guid.NewGuid();
            var player = EntitiesFixture.CreateDefaultPlayer(EntitiesFixture.CreateDefaultHero(), Guid.NewGuid());
            var created = DateTime.UtcNow;
            var battleState = BattleState.InAction(battleId, player, created);
            var expectedException = new InvalidPlayerException();

            var exception = Record.Exception(() => battleState.UpdatePlayer(null, DateTime.UtcNow));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_null_hero_when_update_player_should_throw_an_exception()
        {
            var battleId = Guid.NewGuid();
            var player = Player.Create("test", null, 1000, Guid.NewGuid());
            var battleState = BattleState.InAction(battleId, player, DateTime.UtcNow);
            var expectedException = new InvalidPlayerException();

            var exception = Record.Exception(() => battleState.UpdatePlayer(player, DateTime.UtcNow));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        public static TheoryData<BattleState> BattleStateData
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
    }
}
