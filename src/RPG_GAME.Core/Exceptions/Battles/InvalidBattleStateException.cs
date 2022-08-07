namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class InvalidBattleStateException : DomainException
    {
        public InvalidBattleStateException() : base("INvalid BattleState")
        {
        }
    }
}
