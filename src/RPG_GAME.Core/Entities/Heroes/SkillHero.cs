using System;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Exceptions.Heroes;

namespace RPG_GAME.Core.Entities.Heroes
{
    public class SkillHero : SkillHero<int>
    {
        public SkillHero(Guid id, string name, int baseAttack, IncreasingState<int> increasingState) : base(id, name, baseAttack, increasingState)
        {
        }

        public override void ChangeSkillBaseAttack(int baseAttack)
        {
            if (baseAttack is < 0)
            {
                throw new InvalidSkillHeroAttackException();
            }

            base.ChangeSkillBaseAttack(baseAttack);
        }

        public new static SkillHero Create(string name, int baseAttack, IncreasingState<int> increasingState)
        {
            return new SkillHero(Guid.NewGuid(), name, baseAttack, increasingState);
        }
}

    public class SkillHero<T>
        where T : struct
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public virtual T BaseAttack { get; private set; }
        public IncreasingState<T> IncreasingState { get; private set; }

        public SkillHero(Guid id, string name, T baseAttack, IncreasingState<T> increasingState)
        {
            Id = id;
            ChangeSkillName(name);
            ChangeSkillBaseAttack(baseAttack);
            ChangeSkillBaseAttack(baseAttack);
            ChangeSkillIncreasingState(increasingState);
        }

        public static SkillHero<T> Create(string name, T baseAttack, IncreasingState<T> increasingState)
        {
            return new SkillHero<T>(Guid.NewGuid(), name, baseAttack, increasingState);
        }

        public void ChangeSkillName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidSkillHeroNameException();
            }

            Name = name;
        }

        public virtual void ChangeSkillBaseAttack(T baseAttack)
        {
            BaseAttack = baseAttack;
        }

        public void ChangeSkillIncreasingState(IncreasingState<T> increasingState)
        {
            if (increasingState is null)
            {
                throw new InvalidSkillHeroIncreasingStateException();
            }

            IncreasingState = increasingState;
        }
    }
}
