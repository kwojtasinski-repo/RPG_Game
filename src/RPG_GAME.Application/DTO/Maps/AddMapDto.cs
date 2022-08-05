namespace RPG_GAME.Application.DTO.Maps
{
    public class AddMapDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Difficulty { get; set; }
        public IEnumerable<AddEnemyDto> Enemies { get; set; }
    }

    public class AddEnemyDto
    {
        public Guid EnemyId { get; set; }
        public int Quantity { get; set; }
    }
}
