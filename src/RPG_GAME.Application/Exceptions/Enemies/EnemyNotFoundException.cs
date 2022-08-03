namespace RPG_GAME.Application.Exceptions.Enemies
{
    internal sealed class EnemyNotFoundException : BusinessException
    {
        public Guid Id { get; }

        public EnemyNotFoundException(Guid id) : base($"Enemy with id '{id}' was not found.")
        {
            Id = id;
        }
    }
}
