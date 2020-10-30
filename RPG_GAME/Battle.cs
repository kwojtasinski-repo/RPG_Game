using System;
using System.Runtime.Serialization;

namespace RPG_GAME
{
    public static class Battle
    {
        public static bool IsHeroDead(Hero hero)
        {
            if (hero.Health <= 0)
            {
                //Console.Clear();
                Console.WriteLine("You are dead!");
                Console.WriteLine("Better luck next time!");
                Console.ReadLine();
                throw new MyException("Game is over. Thanks for playing");
            }
            return false;
        }

        public static void PrintTheStats(Person person1, Person person2)
        {
            person1.PrintStats();
            Console.WriteLine("");
            person2.PrintStats();
            Console.WriteLine("");
        }


        public static void Fight(Hero hero, Enemy enemy)
        {
            while (enemy.Health > 0 && hero.Health > 0 && !IsHeroDead(hero))
            {
                PrintTheStats(enemy, hero);
                Random rnd = new Random();

                hero.HeroTurn(hero.ChoiceAction(), enemy);

                if (enemy.Health > 0)
                {
                    enemy.EnemyTurn(rnd.Next(1, 3), hero);
                    IsHeroDead(hero);
                }

            }
        }

        public static void WithArcher(Hero hero, Archer archer)
        {
            Fight(hero, archer);

            if (IsHeroDead(hero))
                return;

            Console.WriteLine($"You killed {archer.Name}");
            Console.WriteLine("Gained +20 exp");
            hero.GainExp(20);
            if (hero.Exp >= 100)
                hero.LevelUp();

            Console.ReadLine();
        }

        public static void WithKnight(Hero hero, Knight knight)
        {
            Fight(hero, knight);

            if (IsHeroDead(hero))
                return;

            Console.WriteLine($"You killed {knight.Name}");
            Console.WriteLine("Gained +30 exp");
            hero.GainExp(30);
            if (hero.Exp >= 100)
                hero.LevelUp();

            Console.ReadLine();
        }

        public static void WithDragon(Hero hero, Dragon dragon)
        {
            Fight(hero, dragon);

            if (IsHeroDead(hero))
                return;

            Console.WriteLine($"You killed {dragon.Name}");
            Console.WriteLine("Gained +50 exp");
            hero.GainExp(50);
            if (hero.Exp >= 100)
                hero.LevelUp();

            Console.ReadLine();
            //Console.Clear();
        }
    }

    [Serializable]
    public class MyException : Exception
    {
        // Constructors
        public MyException(string message)
            : base(message)
        { }

        // Ensure Exception is Serializable
        protected MyException(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        { }
    }
}
