using System;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Enemies;

namespace RPG_GAME.Core.Entities.Enemies
{
    public class SkillEnemy : SkillEnemy<int>
    {
        public SkillEnemy(Guid id, string name, int baseAttack, decimal probability, IncreasingState<int> increasingState) 
            : base(id, name, baseAttack, probability, increasingState)
        {
        }

        public override void ChangeSkillBaseAttack(int baseAttack)
        {
            if (baseAttack is < 0)
            {
                throw new InvalidSkillEnemyAttackException();
            }

            base.ChangeSkillBaseAttack(baseAttack);
        }

        public static new SkillEnemy Create(string name, int baseAttack, decimal probability, IncreasingState<int> increasingState)
        {
            return new SkillEnemy(Guid.NewGuid(), name, baseAttack, probability, increasingState);
        }
    }

    public class SkillEnemy<T>
        where T : struct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual T BaseAttack { get; set; }
        public decimal Probability { get; set; }
        public IncreasingState<T> IncreasingState { get; set; }

        public SkillEnemy(Guid id, string name, T baseAttack, decimal probability, IncreasingState<T> increasingState)
        {
            Id = id;
            ChangeSkillName(name);
            ChangeSkillBaseAttack(baseAttack);
            ChangeSkillBaseAttack(baseAttack);
            ChangeProbability(probability);
            ChangeSkillIncreasingState(increasingState);
        }

        public static SkillEnemy<T> Create(string name, T baseAttack, decimal probability, IncreasingState<T> increasingState)
        {
            return new SkillEnemy<T>(Guid.NewGuid(), name, baseAttack, probability, increasingState);
        }

        public void ChangeSkillName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidSkillEnemyNameException();
            }

            Name = name;
        }

        public virtual void ChangeSkillBaseAttack(T baseAttack)
        {
            BaseAttack = baseAttack;
        }

        public void ChangeProbability(decimal probability)
        {
            if (probability is < 0 or > 95)
            {
                throw new InvalidSkillEnemyProbabilityException();
            }

            Probability = probability;
        }

        public void ChangeSkillIncreasingState(IncreasingState<T> increasingState)
        {
            if (increasingState is null)
            {
                throw new InvalidSkillEnemyIncreasingStateException();
            }

            IncreasingState = increasingState;
        }
    }
}
