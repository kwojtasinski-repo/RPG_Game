
namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class CannotCreateEventForBattleWithInfoException : BusinessException
    {
        public Guid BattleId { get; }
        public string BattleInfo { get; }

        public CannotCreateEventForBattleWithInfoException(Guid battleId, string battleInfo) : base($"Cannot create BattleEvent for Battle with id: '{battleId}' and BattleInfo '{battleInfo}'")
        {
            BattleId = battleId;
            BattleInfo = battleInfo;
        }
    }
}
