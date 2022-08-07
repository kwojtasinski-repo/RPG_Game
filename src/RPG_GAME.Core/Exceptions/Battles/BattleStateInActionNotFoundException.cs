namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class BattleStateInActionNotFoundException : DomainException
    {
        public BattleStateInActionNotFoundException() : base("BattleState with status 'InAction' was not found")
        {
        }
    }
}
