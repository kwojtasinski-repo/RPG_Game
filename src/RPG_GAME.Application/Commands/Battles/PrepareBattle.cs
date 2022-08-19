using RPG_GAME.Application.DTO.Battles;

namespace RPG_GAME.Application.Commands.Battles
{
    public class PrepareBattle : ICommand<BattleDetailsDto>
    {
        public Guid UserId { get; set; }
        public Guid MapId { get; set; }
    }
}
