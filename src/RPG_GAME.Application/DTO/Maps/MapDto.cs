namespace RPG_GAME.Application.DTO.Maps
{
    public class MapDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public IEnumerable<EnemiesDto> Enemies { get; set; }
    }
}
