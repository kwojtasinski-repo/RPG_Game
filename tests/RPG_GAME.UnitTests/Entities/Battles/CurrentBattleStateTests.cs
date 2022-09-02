using FluentAssertions;
using RPG_GAME.Core.Entities.Battles;
using System;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Battles
{
    public class CurrentBattleStateTests
    {
        [Fact]
        public void should_create_current_battle_state()
        {
            var id = Guid.NewGuid();
            var battleId = Guid.NewGuid();
            var playerId = Guid.NewGuid();
            var playerCurrentHealth = 10;
            var playerLevel = 1;
            var enemyId = Guid.NewGuid();
            var enemyHealth = 5;
            var modifiedDate = DateTime.UtcNow;
            var enemiesKilled = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            var currentBattleState = new CurrentBattleState(id, battleId, playerId, playerCurrentHealth, playerLevel,
                    enemyId, enemyHealth, modifiedDate, enemiesKilled);

            currentBattleState.Should().NotBeNull();
            currentBattleState.BattleId.Should().Be(battleId);
            currentBattleState.PlayerId.Should().Be(playerId);
            currentBattleState.PlayerCurrentHealth.Should().Be(playerCurrentHealth);
            currentBattleState.PlayerLevel.Should().Be(playerLevel);
            currentBattleState.EnemyId.Should().Be(enemyId);
            currentBattleState.EnemyHealth.Should().Be(enemyHealth);
            currentBattleState.ModifiedDate.Should().Be(modifiedDate);
            currentBattleState.EnemiesKilled.Should().HaveCount(enemiesKilled.Count);
            currentBattleState.EnemiesKilled.Should().Contain(enemiesKilled);
        }
    }
}
