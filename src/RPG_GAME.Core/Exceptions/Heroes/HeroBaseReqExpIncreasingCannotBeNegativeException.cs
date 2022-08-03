namespace RPG_GAME.Core.Exceptions.Heroes
{
    internal sealed class HeroBaseReqExpIncreasingCannotBeNegativeException : DomainException
    {
        public decimal IncreasingState { get; }

        public HeroBaseReqExpIncreasingCannotBeNegativeException(decimal increasingState) : base($"Hero baseRequiredExperience IncreasingState: '{increasingState}' cannot be negative")
        {
            IncreasingState = increasingState;
        }
    }
}
