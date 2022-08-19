namespace RPG_GAME.Application.Exceptions.Battles
{
    internal sealed class BattleNotFoundException : BusinessException
    {
        public Guid BattleId { get; }

        public BattleNotFoundException(Guid battleId) : base($"Battle with id: '{battleId}' was not found")
        {
            BattleId = battleId;
        }
    }
}
