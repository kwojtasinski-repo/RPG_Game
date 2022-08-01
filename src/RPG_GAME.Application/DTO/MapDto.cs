using RPG_GAME.Core.Entities;

namespace RPG_GAME.Application.DTO
{
    public class MapDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public IList<RequiredEnemyDto> Enemies { get; set; }
    }
}
