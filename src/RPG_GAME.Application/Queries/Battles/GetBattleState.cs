using RPG_GAME.Application.DTO.Battles;

namespace RPG_GAME.Application.Queries.Battles
{
    public class GetBattleState : IQuery<BattleStateDto>
    {
        public Guid BattleId { get; set; }
    }
}
