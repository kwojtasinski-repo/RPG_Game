namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroBaseReqExpCannotBeNegativeException : DomainException
    {
        public decimal BaseRequiredExperience { get; }

        public HeroBaseReqExpCannotBeNegativeException(decimal baseRequiredExperience) : base($"Hero BaseRequiredExperience: '{baseRequiredExperience}' cannot be negative")
        {
            BaseRequiredExperience = baseRequiredExperience;
        }
    }
}
