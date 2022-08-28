using RPG_GAME.Application.DTO.Battles;

namespace RPG_GAME.Application.Commands.Battles
{
    public class AddBattleEvent : ICommand<BattleEventDto>
    {
        public Guid BattleId { get; set; }
        public Guid PlayerId { get; set; }
        public Guid EnemyId { get; set; }
        public string Action { get; set; }
    }
}
