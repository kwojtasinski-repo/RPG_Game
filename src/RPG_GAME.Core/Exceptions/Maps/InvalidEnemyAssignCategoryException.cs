namespace RPG_GAME.Core.Exceptions.Maps
{
    internal sealed class InvalidEnemyAssignCategoryException : DomainException
    {
        public string Category { get; }

        public InvalidEnemyAssignCategoryException(string category) : base($"Invalid EnemyAssign Category '{category}'")
        {
            Category = category;
        }
    }
}
