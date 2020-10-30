using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME
{
    class Program
    {
        static void Main(string[] args)
        {
            int try_hard = 0;
            Hero Dash = new Hero("Dash", 10, 15);
            int try_again=1;
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
                while (try_again == 1)
                {
                    if (try_hard > 0)
                    {
                        Console.Clear();
                        Console.WriteLine("Let's try again");
                        ResetEnemies(listOfEnemies);
                        UpgradeEnemies(listOfEnemies, new UpgradeEnemy("Archer", try_hard+2, try_hard+3, try_hard+1));
                        UpgradeEnemies(listOfEnemies, new UpgradeEnemy("Knight", try_hard + 4, try_hard + 4, try_hard + 2));
                        UpgradeEnemies(listOfEnemies, new UpgradeEnemy("Dragon", try_hard + 5, try_hard + 6, try_hard + 7));
                    }

                    Story.Begin();
                    FindEnemies(listOfEnemies, "Archer").ForEach(t => Battle.WithArcher(Dash, t as Archer));
                    //  Battle.WithArcher(Dash, Sworn);
                    //  Battle.WithArcher(Dash, Beastly);

                    Story.BeforeKnights();
                    FindEnemies(listOfEnemies, "Knight").ForEach(t => Battle.WithKnight(Dash, t as Knight));
                    //  Battle.WithKnight(Dash, Fearless);
                    //  Battle.WithKnight(Dash, Invincible);

                    Story.BeforeDragons();
                    FindEnemies(listOfEnemies, "Dragon").ForEach(t => Battle.WithDragon(Dash, t as Dragon));
                    //  Battle.WithDragon(Dash, BlueDrag);
                    //  Battle.WithDragon(Dash, RedDrag);
                    //  Battle.WithDragon(Dash, BlackDrag);

                    Story.TheEnd();
                    Console.ReadLine();
                    try_hard++;

                    Console.WriteLine("Do you wish to continue? 1=Yes, 2=No (Default No)");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            try_again = 1;
                            break;
                        case "2":
                            try_again = 2;
                            break;
                        default:
                            try_again = 2;
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
