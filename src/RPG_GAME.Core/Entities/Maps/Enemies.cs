using RPG_GAME.Core.Exceptions.Maps;

namespace RPG_GAME.Core.Entities.Maps
{
    public class Enemies
    {
        public EnemyAssign Enemy { get; }
        public int Quantity { get; private set; }

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

        public void AddQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new QuantityCannotBeZeroOrNegative(quantity);
            }

            Quantity += quantity;
        }

        public void RemoveQuantity(int quantity)
        {
            if (quantity <= 0)
            {
                throw new QuantityCannotBeZeroOrNegative(quantity);
            }

            var quantitySubtract = Quantity - quantity;

            if (quantitySubtract <= 0)
            {
                throw new QuantityCannotBeZeroOrNegative(quantitySubtract);
            }

            Quantity = quantitySubtract;
        }
    }
}
