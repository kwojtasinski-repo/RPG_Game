using RPG_GAME.Application.DTO.Battles;
using RPG_GAME.Core.Entities.Maps;

namespace RPG_GAME.Application.Managers
{
    internal sealed class EnemyAttackManager : IEnemyAttackManager
    {
        private IDictionary<string, AttackWithProbability> _enemyAttacks = new Dictionary<string, AttackWithProbability>();

        public EnemyAttackDto AttackHeroWithDamage(EnemyAssign enemyAssign)
        {
            AddAttacks(enemyAssign);
            return CalculateAttack(enemyAssign);
        }

        private void AddAttacks(EnemyAssign enemyAssign)
        {
            foreach (var skill in enemyAssign.Skills)
            {
                _enemyAttacks.Add(skill.Name, new AttackWithProbability(skill.Attack, skill.Name, skill.Probability));
            }

            _enemyAttacks = _enemyAttacks.OrderByDescending(e => e.Value.Probability)
                            .ToDictionary(ea => ea.Key, ea => ea.Value);
        }

        private record AttackWithProbability(int Attack, string AttackName, decimal Probability);

        private EnemyAttackDto CalculateAttack(EnemyAssign enemyAssign)
        {
            var count = _enemyAttacks.Count();

            if (count == 0)
            {
                return BaseAttack(enemyAssign);
            }

            var random = new Random();
            var value = random.Next(1, count);
            var attack = _enemyAttacks.ElementAt(value);
            var skill = enemyAssign.Skills.Where(a => a.Name == attack.Key);

            if (skill is null)
            {
                return BaseAttack(enemyAssign);
            }

            var difference = 100 - (int) attack.Value.Probability;
            var number = random.Next(1, difference);

            if (number != difference)
            {
                return BaseAttack(enemyAssign);
            }

            return new EnemyAttackDto
            {
                AttackName = attack.Value.AttackName,
                Damage = attack.Value.Attack
            };
        }

        private EnemyAttackDto BaseAttack(EnemyAssign enemyAssign)
        {
            return new EnemyAttackDto
            {
                AttackName = nameof(EnemyAssign.EnemyName),
                Damage = enemyAssign.Attack
            };
        }
    }
}
