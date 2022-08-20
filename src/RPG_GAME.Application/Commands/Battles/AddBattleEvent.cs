using RPG_GAME.Application.DTO.Battles;

namespace RPG_GAME.Application.Commands.Battles
{
    public class AddBattleEvent : ICommand<BattleEventDto>
    {
        public Guid BattleId { get; set; }
        public Guid PlayerId { get; set; }
        public Guid EnemyId { get; set; }
        public int DamageDealt { get; set; }
        public string AttackInfo { get; set; }
    }
}
