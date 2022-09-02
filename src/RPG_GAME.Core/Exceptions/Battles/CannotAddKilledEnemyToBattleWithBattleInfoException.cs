using RPG_GAME.Core.Entities.Battles;

namespace RPG_GAME.Core.Exceptions.Battles
{
    internal sealed class CannotAddKilledEnemyToBattleWithBattleInfoException : DomainException
    {
        public BattleInfo BattleInfo { get; }

        public CannotAddKilledEnemyToBattleWithBattleInfoException(BattleInfo battleInfo) : base($"Cannot add killed enemy to battle with BattleInfo '{battleInfo}'")
        {
            BattleInfo = battleInfo;
        }
    }
}
