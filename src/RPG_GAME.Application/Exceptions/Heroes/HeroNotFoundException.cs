namespace RPG_GAME.Application.Exceptions.Heroes
{
    public sealed class HeroNotFoundException : BusinessException
    {
        public Guid Id { get; }

        public HeroNotFoundException(Guid id) : base($"Hero with id '{id}' was not found.")
        {
            Id = id;
        }
    }
}
