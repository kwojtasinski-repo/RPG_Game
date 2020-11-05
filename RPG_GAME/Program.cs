using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME
{
    class Program
    {
        static void Main(string[] args)
        {
            int tryHard = 0;
            Hero hero = new Hero("Warrior", 10, 15);
            int tryAgain=1;
            Archer Sworn = new Archer("Sworn Archer", 1, 5, "Archer");
            Archer Beastly = new Archer("Beastly Archer", 2, 8, "Archer");

            Knight Fearless = new Knight("Fearless Knight", 3, 12, "Knight");
            Knight Invincible = new Knight("Invincible Knight", 5, 15, "Knight");

            Dragon BlueDrag = new Dragon("Blue Dragon", 7, 20, "Dragon");
            Dragon RedDrag = new Dragon("Red Dragon", 8, 20, "Dragon");
            Dragon BlackDrag = new Dragon("Black Dragon", 10, 25, "Dragon");

            List<Enemy> listOfEnemies = new List<Enemy>();
            listOfEnemies.Add(Sworn);
            listOfEnemies.Add(Beastly);
            listOfEnemies.Add(Fearless);
            listOfEnemies.Add(Invincible);
            listOfEnemies.Add(BlueDrag);
            listOfEnemies.Add(RedDrag);
            listOfEnemies.Add(BlackDrag);

            try 
            {
                while (tryAgain == 1)
                {
                    if (tryHard > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Let's try again");
                        ResetEnemies(listOfEnemies);
                        UpgradeEnemies(listOfEnemies, new UpgradeEnemy("Archer", tryHard+2, tryHard+3, tryHard+1));
                        UpgradeEnemies(listOfEnemies, new UpgradeEnemy("Knight", tryHard + 4, tryHard + 4, tryHard + 2));
                        UpgradeEnemies(listOfEnemies, new UpgradeEnemy("Dragon", tryHard + 5, tryHard + 6, tryHard + 7));
                    }

                    Story.Begin();
                    FindEnemies(listOfEnemies, "Archer").ForEach(t => Battle.WithArcher(hero, t as Archer));

                    Story.BeforeKnights();
                    FindEnemies(listOfEnemies, "Knight").ForEach(t => Battle.WithKnight(hero, t as Knight));

                    Story.BeforeDragons();
                    FindEnemies(listOfEnemies, "Dragon").ForEach(t => Battle.WithDragon(hero, t as Dragon));

                    Story.TheEnd();
                    Console.ReadLine();
                    tryHard++;

                    Console.WriteLine("Do you wish to continue? 1=Yes, 2=No (Default No)");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            tryAgain = 1;
                            break;
                        case "2":
                            tryAgain = 2;
                            break;
                        default:
                            tryAgain = 2;
                            break;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Press to continue...");
            Console.ReadLine();
        }

        public static void UpgradeEnemies(List<Enemy> listOfEnemies, UpgradeEnemy upgradeEnemy)
        {
            List<Enemy> enemies = FindEnemies(listOfEnemies, upgradeEnemy.Category);
            (from u in listOfEnemies where u.Category == upgradeEnemy.Category select u).ToList().ForEach(u =>
            {
                u.UpgradeAttack(upgradeEnemy.UpgradeAttack);
                u.UpgradeHealth(upgradeEnemy.UpgradeHealth);
                u.UpgradeMaxHealth(upgradeEnemy.UpgradeHealth);
                u.UpgradeHeal(upgradeEnemy.UpgradeHeal);
            }
            );

        }
        public static List<Enemy> FindEnemies(List<Enemy> listOfEnemies, string Category)
        {
            return listOfEnemies.FindAll(enemies => enemies.Category == Category);
        }

        public static void ResetEnemies(List<Enemy> listOfEnemies)
        {
            listOfEnemies.ForEach(t => t.Reset());
        }
    }

    public class UpgradeEnemy
    {
        private string _category;
        private int _upgrade_attack;
        private int _upgrade_health;
        private int _upgrade_heal;
        public string Category { get { return _category; } }
        public int UpgradeAttack { get { return _upgrade_attack; } }
        public int UpgradeHealth { get { return _upgrade_health; } }
        public int UpgradeHeal { get { return _upgrade_heal; } }
        public UpgradeEnemy(string category, int upgrade_attack, int upgrade_health, int upgrade_heal)
        {
            _category = category;
            _upgrade_attack = upgrade_attack;
            _upgrade_health = upgrade_health;
            _upgrade_heal = upgrade_heal;
        }
    }

}
