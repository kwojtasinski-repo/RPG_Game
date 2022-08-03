namespace RPG_GAME.Application.DTO.Players
{
    public class AddPlayerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid HeroId { get; set; }
        public Guid UserId { get; set; }
    }
}
