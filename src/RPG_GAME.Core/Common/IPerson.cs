
namespace RPG_GAME.Core.Common
{
    public interface IPerson
    {
        int MaxHealth { get; set; }
        string Name { get; set; }
        int Health { get; set; }
        int Attack { get; set; }

        void ChangeName(string name);

        void UpgradeAttack(int attack);

        void UpgradeMaxHealth(int health);
        void UpgradeHeal(int heal);

        void ReceiveDmg(int dmg);

        void SetHealth(int health);

        void PrintStats();

        void NormAttack(IPerson target);

        void Heal();

        void Reset();

    }
}
