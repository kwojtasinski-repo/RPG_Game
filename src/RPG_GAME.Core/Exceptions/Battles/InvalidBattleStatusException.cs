namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class InvalidBattleStatusException : DomainException
    {
        public string BattleStatus { get; }

        public InvalidBattleStatusException(string battleStatus) : base($"Invalid BattleStatus '{battleStatus}'")
        {
            BattleStatus = battleStatus;
        }
    }
}
