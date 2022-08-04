namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroBaseReqExpCannotBeZeroOrNegativeException : DomainException
    {
        public decimal BaseRequiredExperience { get; }

        public HeroBaseReqExpCannotBeZeroOrNegativeException(decimal baseRequiredExperience) : base($"Hero BaseRequiredExperience: '{baseRequiredExperience}' cannot be zero or negative")
        {
            BaseRequiredExperience = baseRequiredExperience;
        }
    }
}
