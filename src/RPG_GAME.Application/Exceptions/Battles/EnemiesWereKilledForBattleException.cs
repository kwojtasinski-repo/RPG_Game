namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class EnemiesWereKilledForBattleException : BusinessException
    {
        public Guid BattleId { get; }

        public EnemiesWereKilledForBattleException(Guid battleId) : base($"All enemies were killed for battle with id: '{battleId}'")
        {
            BattleId = battleId;
        }
    }
}
