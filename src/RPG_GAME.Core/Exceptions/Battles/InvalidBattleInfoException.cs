namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class InvalidBattleInfoException : DomainException
    {
        public string BattleInfo { get; }

        public InvalidBattleInfoException(string battleInfo) : base($"Invalid BattleInfo '{battleInfo}'")
        {
            BattleInfo = battleInfo;
        }
    }
}
