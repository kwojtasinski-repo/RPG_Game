namespace RPG_GAME.Application.DTO.Battles
{
    public class BattleDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public Guid UserId { get; set; }
        public string BattleInfo { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
