namespace RPG_GAME.Core.Exceptions.Players
{
    internal sealed class PlayerCurrentExpCannotBeZeroOrNegativeException : DomainException
    {
        public decimal CurrentExp { get; }

        public PlayerCurrentExpCannotBeZeroOrNegativeException(decimal currentExp) : base($"CurrentExp: '{currentExp}' cannot be zero or negative")
        {
            CurrentExp = currentExp;
        }
    }
}
