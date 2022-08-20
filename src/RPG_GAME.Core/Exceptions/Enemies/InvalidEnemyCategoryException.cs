namespace RPG_GAME.Core.Exceptions.Enemies
{
    internal sealed class InvalidEnemyCategoryException : DomainException
    {
        public string Category { get; }

        public InvalidEnemyCategoryException(string category) : base($"Invalid Enemy Category: '{category}'")
        {
            Category = category;
        }
    }
}
