namespace RPG_GAME.Application.Exceptions.Enemies
{
    public class EnemiesShouldntHaveDuplicatedEnemyIdException : BusinessException
    {
        public EnemiesShouldntHaveDuplicatedEnemyIdException() : base("Enemies shouldnt have duplicated EnemyId")
        {
        }
    }
}
