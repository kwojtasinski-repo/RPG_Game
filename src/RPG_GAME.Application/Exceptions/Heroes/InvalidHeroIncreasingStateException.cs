namespace RPG_GAME.Application.Exceptions.Heroes
{
    public sealed class InvalidHeroIncreasingStateException : BusinessException
    {
        public string Name { get; }

        public InvalidHeroIncreasingStateException(string name) : base("Invalid hero increasing state '{name}'")
        {
            Name = name;
        }
    }
}
