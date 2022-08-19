using RPG_GAME.Application.DTO.Players;

namespace RPG_GAME.Application.DTO.Battles
{
    public class BattleStateDto
    {
        public Guid Id { get; set; }
        public Guid BattleId { get; set; }
        public string BattleStatus { get; set; }
        public PlayerDto Player { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
