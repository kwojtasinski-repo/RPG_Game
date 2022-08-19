using RPG_GAME.Application.DTO.Battles;

namespace RPG_GAME.Application.Commands.Battles
{
    public class StartBattle : ICommand<BattleDetailsDto>
    {
        public Guid BattleId { get; set; }
        public Guid UserId { get; set; }
    }
}
