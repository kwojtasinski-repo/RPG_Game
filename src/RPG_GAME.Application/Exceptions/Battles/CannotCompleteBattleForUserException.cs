namespace RPG_GAME.Application.Exceptions.Battles
{
    internal class CannotCompleteBattleForUserException : BusinessException
    {
        public Guid BattleId { get; }
        public Guid UserId { get; }

        public CannotCompleteBattleForUserException(Guid battleId, Guid userId) : base($"Cannot complete battle with id: '{battleId}' for user with id: '{userId}'")
        {
            BattleId = battleId;
            UserId = userId;
        }
    }
}
