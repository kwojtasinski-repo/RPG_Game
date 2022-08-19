namespace RPG_GAME.Application.Exceptions.Battles
{
    internal sealed class CannotStartBattleForUserException : BusinessException
    {
        public Guid BattleId { get; }
        public Guid UserId { get; }

        public CannotStartBattleForUserException(Guid battleId, Guid userId) : base($"Cannot start battle with id: '{battleId}' for user with id: '{userId}'")
        {
            BattleId = battleId;
            UserId = userId;
        }
    }
}
