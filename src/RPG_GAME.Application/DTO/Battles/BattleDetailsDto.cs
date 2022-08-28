using RPG_GAME.Application.DTO.Maps;
using System.Collections.Immutable;

namespace RPG_GAME.Application.DTO.Battles
{
    public class BattleDetailsDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public Guid UserId { get; set; }
        public string BattleInfo { get; set; }
        public DateTime? EndDate { get; set; }
        public MapDto Map { get; set; }
        public IEnumerable<BattleStateDto> BattleStates { get; set; }
        public IImmutableDictionary<Guid, int> EnemiesKilled { get; set; }
    }
}
