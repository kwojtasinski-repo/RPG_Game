using RPG_GAME.Core.Exceptions.Maps;

namespace RPG_GAME.Core.Entities.Maps
{
    public class Enemies
    {
        public EnemyAssign Enemy { get; }
        public int Quantity { get; }

        public Enemies(EnemyAssign enemy, int quantity)
        {
            if (enemy is null)
            {
                throw new InvalidEnemyException();
            }

            if (quantity <= 0)
            {
                throw new QuantityCannotBeZeroOrNegative(quantity);
            }

            Enemy = enemy;
            Quantity = quantity;
        }
    }
}
