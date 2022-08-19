namespace RPG_GAME.Application.DTO.Battles
{
    public class BattleEventDto
    {
        public Guid Id { get; set; }
        public Guid BattleId { get; set; }
        public FightActionDto Action { get; set; }
        public DateTime Created { get; set; }
    }
}
