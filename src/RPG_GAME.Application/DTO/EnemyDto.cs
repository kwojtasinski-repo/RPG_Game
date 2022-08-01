using RPG_GAME.Core.Entities;

namespace RPG_GAME.Application.DTO
{
    public class EnemyDto
    {
        public Guid Id { get; set; }
        public string EnemyName { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
