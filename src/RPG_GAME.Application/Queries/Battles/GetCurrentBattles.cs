using RPG_GAME.Application.DTO.Battles;

namespace RPG_GAME.Application.Queries.Battles
{
    public class GetCurrentBattles : IQuery<IEnumerable<BattleDto>>
    {
        public Guid UserId { get; set; }
    }
}
