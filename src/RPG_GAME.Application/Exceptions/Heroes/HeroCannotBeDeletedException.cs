namespace RPG_GAME.Application.Exceptions.Heroes
{
    internal sealed class HeroCannotBeDeletedException : BusinessException
    {
        public Guid Id { get; }

        public HeroCannotBeDeletedException(Guid id) : base($"Hero with id '{id}' cannot be deleted")
        {
            Id = id;
        }
    }
}
