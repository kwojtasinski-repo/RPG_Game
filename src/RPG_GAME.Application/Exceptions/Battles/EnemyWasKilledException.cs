namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class EnemyWasKilledException : BusinessException
    {
        public Guid EnemyId { get; }
        public int EnemyKilledTimes { get; }

        public EnemyWasKilledException(Guid enemyId, int enemyKilledTimes) :base($"Enemy with id: '{enemyId}' was killed '{enemyKilledTimes}' times")
        {
            EnemyId = enemyId;
            EnemyKilledTimes = enemyKilledTimes;
        }
    }
}
