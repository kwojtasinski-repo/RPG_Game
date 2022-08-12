using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Maps;

namespace RPG_GAME.Application.Events.Enemies
{
    internal class EnemyUpdated : IEvent
    {
        public Guid EnemyId { get; }
        public string EnemyName { get; }
        public int BaseAttack { get; }
        public int BaseHealth { get; }
        public int BaseHealLvl { get; }
        public decimal Experience { get; }
        public Difficulty Difficulty { get; }
        public IEnumerable<SkillEnemyAssign> Skills { get; } = new List<SkillEnemyAssign>();

        public EnemyUpdated(Guid enemyId, string enemyName, int baseAttack, int baseHealth, int baseHealLvl, decimal experience, Difficulty difficulty, IEnumerable<SkillEnemyAssign> skills = null)
        {
            EnemyId = enemyId;
            EnemyName = enemyName;
            BaseAttack = baseAttack;
            BaseHealth = baseHealth;
            BaseHealLvl = baseHealLvl;
            Experience = experience;
            Difficulty = difficulty;

            if (skills is not null)
            {
                Skills = skills;
            }
        }

    }
}
