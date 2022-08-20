namespace RPG_GAME.Application.Exceptions.Battles
{
    public sealed class InvalidAttackException : BusinessException
    {
        public string AttackName { get; }

        public InvalidAttackException(string attackName) : base($"Invalid attack: '{attackName}'")
        {
            AttackName = attackName;
        }
    }
}
