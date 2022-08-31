namespace RPG_GAME.Application.Exceptions.Heroes
{
    public sealed class HeroNotFoundException : BusinessException
    {
        public Guid HeroId { get; }

        public HeroNotFoundException(Guid heroId) : base($"Hero with id '{heroId}' was not found.")
        {
            HeroId = heroId;
        }
    }
}
