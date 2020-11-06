using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Common;
using System.Collections.Generic;

namespace RPG_GAME.Service.Concrete
{
    public class EnemyService : BaseService<Enemy>
    {
        public EnemyService()
        {
            Initialize();
        }

        public List<Enemy> GetEnemiesByDiffLvl(int difLvl)
        {
            List<Enemy> result = new List<Enemy>();
            foreach (var enemy in Objects)
            {
                if (enemy.DiffLvl == difLvl)
                {
                    result.Add(enemy);
                }
            }
            return result;
        }

        public List<Enemy> FindEnemiesByCategory(List<Enemy> listOfEnemies, string Category)
        {
            return listOfEnemies.FindAll(enemies => enemies.Category == Category);
        }

        public void ResetEnemies()
        {
            foreach (Enemy enemy in Objects)
            {
                if (enemy.Health < enemy.MaxHealth)
                    enemy.SetHealth(enemy.MaxHealth);
            }
                
        }

        private void Initialize()
        {
            AddObject(new Archer("Sworn Archer", 1, 5, "Archer", 1));
            AddObject(new Archer("Sworn Archer", 3, 10, "Archer", 2, 1));
            AddObject(new Archer("Sworn Archer", 5, 15, "Archer", 3, 2));
            AddObject(new Archer("Beastly Archer", 2, 8, "Archer", 1));
            AddObject(new Archer("Beastly Archer", 5, 12, "Archer", 2, 1));
            AddObject(new Archer("Beastly Archer", 7, 18, "Archer", 3, 2));
            
            AddObject(new Knight("Fearless Knight", 3, 12, "Knight", 1));
            AddObject(new Knight("Fearless Knight", 7, 18, "Knight", 2, 2));
            AddObject(new Knight("Fearless Knight", 12, 22, "Knight", 3, 3));
            AddObject(new Knight("Invincible Knight", 5, 15, "Knight", 1));
            AddObject(new Knight("Invincible Knight", 10, 20, "Knight", 2, 2));
            AddObject(new Knight("Invincible Knight", 15, 25, "Knight", 3, 3));
            
            AddObject(new Dragon("Blue Dragon", 7, 20, "Dragon", 1));
            AddObject(new Dragon("Blue Dragon", 10, 25, "Dragon", 2, 4));
            AddObject(new Dragon("Blue Dragon", 17, 30, "Dragon", 3, 5));
            AddObject(new Dragon("Red Dragon", 8, 20, "Dragon", 1));
            AddObject(new Dragon("Red Dragon", 12, 25, "Dragon", 2, 5));
            AddObject(new Dragon("Red Dragon", 22, 35, "Dragon", 3, 6));
            AddObject(new Dragon("Black Dragon", 10, 25, "Dragon",1));
            AddObject(new Dragon("Black Dragon", 15, 35, "Dragon",2, 7));
            AddObject(new Dragon("Black Dragon", 25, 45, "Dragon",3, 10));
        }

        public void UpgradeEnemies(int upgrade)
        {
            foreach(var enemy in Objects)
            {
                enemy.UpgradeAttack(upgrade*2);
                enemy.UpgradeMaxHealth(upgrade*5);
                enemy.SetHealth(enemy.MaxHealth);
                enemy.UpgradeHeal(upgrade);
            }
        }
    }
}
