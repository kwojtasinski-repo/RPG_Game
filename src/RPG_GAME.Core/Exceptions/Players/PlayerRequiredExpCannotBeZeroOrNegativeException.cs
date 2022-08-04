namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class PlayerRequiredExpCannotBeZeroOrNegativeException : DomainException
    {
        public decimal RequiredExp { get; }

        public PlayerRequiredExpCannotBeZeroOrNegativeException(decimal requiredExp) : base($"RequiredExp: '{requiredExp}' cannot be zero or negative")
        {
            RequiredExp = requiredExp;
        }
    }
}
