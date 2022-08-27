using System;

namespace RPG_GAME.Core.Entities.Battles
{
    public class CurrentBattleState
    {
        public Guid Id { get; }
        public Guid BattleId { get; }
        public Guid PlayerId { get; }
        public int PlayerCurrentHealth { get; private set; }
        public int PlayerLevel { get; private set; }
        public Guid EnemyId { get; }
        public int EnemyHealth { get; private set; }
        public DateTime ModifiedDate { get; }

        public CurrentBattleState(Guid id, Guid battleId, Guid playerId, int playerCurrentHealth,
            int playerLevel, Guid enemyId, int enemyHealth, DateTime modifiedDate)
        {
            Id = id;
            BattleId = battleId;
            PlayerId = playerId;
            PlayerCurrentHealth = playerCurrentHealth;
            PlayerLevel = playerLevel;
            EnemyId = enemyId;
            EnemyHealth = enemyHealth;
            ModifiedDate = modifiedDate;
        }

        public void HealPlayerBy(int heal)
        {
            PlayerCurrentHealth += heal;
        }

        public void MakeDamageToPlayer(int damage)
        {
            PlayerCurrentHealth -= damage;
        }

        public void HealEnemyBy(int heal)
        {
            EnemyHealth += heal;
        }

        public void MakeDamageToEnemy(int damage)
        {
            EnemyHealth -= damage;
        }
    }
}
