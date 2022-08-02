namespace RPG_GAME.Application.DTO.Maps
{
    public class MapDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public IList<RequiredEnemyDto> Enemies { get; set; }
    }
}
