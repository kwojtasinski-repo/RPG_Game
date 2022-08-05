namespace RPG_GAME.Application.Exceptions.Heroes
{
    internal sealed class HeroSkillNotFoundException : BusinessException
    {
        public Guid Id { get; }

        public HeroSkillNotFoundException(Guid id) : base($"HeroSkill with id: '{id}' was not found")
        {
            Id = id;
        }
    }
}
