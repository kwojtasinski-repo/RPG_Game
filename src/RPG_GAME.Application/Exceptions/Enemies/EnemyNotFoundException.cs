namespace RPG_GAME.Application.Exceptions.Enemies
{
    public sealed class EnemyNotFoundException : BusinessException
    {
        public Guid EnemyId { get; }

        public EnemyNotFoundException(Guid enemyId) : base($"Enemy with id '{enemyId}' was not found.")
        {
            EnemyId = enemyId;
        }
    }
}
