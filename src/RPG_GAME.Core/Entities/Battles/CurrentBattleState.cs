using System;

namespace RPG_GAME.Core.Entities.Battles
{
    public class CurrentBattleState
    {
        public Guid Id { get; }
        public Guid BattleId { get; }
        public Guid PlayerId { get; }
        public int PlayerCurrentHealth { get; }
        public int PlayerLevel { get; }
        public Guid EnemyId { get; }
        public int EnemyHealth { get; }

        public CurrentBattleState(Guid id, Guid battleId, Guid playerId, int playerCurrentHealth,
            int playerLevel, Guid enemyId, int enemyHealth)
        {
            Id = id;
            BattleId = battleId;
            PlayerId = playerId;
            PlayerCurrentHealth = playerCurrentHealth;
            PlayerLevel = playerLevel;
            EnemyId = enemyId;
            EnemyHealth = enemyHealth;
        }
    }
}
