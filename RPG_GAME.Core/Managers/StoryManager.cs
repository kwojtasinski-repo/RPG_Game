using RPG_GAME.Core.Common;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace RPG_GAME.Service.Managers
{
    public class StoryManager
    {
        private readonly MenuActionService _actionService;
        private Hero _hero;
        List<Enemy> enemies;
        private EnemyService _enemyService;

        public StoryManager(MenuActionService actionService, Hero hero)
        {
            _actionService = actionService;
            _hero = hero;
            enemies = new List<Enemy>();
            _enemyService = new EnemyService();
        }

        public void Start()
        {
            int diffLvl = ChooseDifficultyLevel();
            enemies = _enemyService.GetEnemiesByDiffLvl(diffLvl);
            Enemy currentEnemy = null;
            try
            {
                var archers = _enemyService.FindEnemiesByCategory(enemies, "Archer");
                Begin(archers.Count);
                foreach (var archer in archers)
                {
                    currentEnemy = archer;
                    BattleWithArcher(_hero, archer as Archer);
                }
                    

                var knights = _enemyService.FindEnemiesByCategory(enemies, "Knight");
                BeforeKnights(knights.Count);
                foreach (var knight in knights)
                {
                    currentEnemy = knight;
                    BattleWithKnight(_hero, knight as Knight);
                }
                    

                var dragons = _enemyService.FindEnemiesByCategory(enemies, "Dragon");
                BeforeDragons(dragons.Count);
                foreach (var dragon in dragons)
                {
                    currentEnemy = dragon;
                    BattleWithDragon(_hero, dragon as Dragon);
                }
                    
                TheEnd();
            }
            catch(Exception e)
            {
                Console.WriteLine("Enemy stats: ");
                currentEnemy.PrintStats();
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("Press enter to continue");
            _enemyService.ResetEnemies();
            Console.ReadLine();
            Console.Clear();
        }

        public int ChooseDifficultyLevel()
        {
            var difficultyLevelMenu = _actionService.GetMenuActionsByMenuName("DifficultyLevel");
            Console.WriteLine("\nPlease select difficulty level:");
            foreach (MenuAction menuAction in difficultyLevelMenu)
            {
                Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
            }

            var operation = Console.ReadKey();
            int diffLvl;
            Int32.TryParse(operation.KeyChar.ToString(), out diffLvl);

            while (diffLvl < 1 || diffLvl > difficultyLevelMenu.OrderBy(o => o.Id).LastOrDefault().Id)
            {
                Console.WriteLine("\nBad Type difficulty level");
                Console.WriteLine("Please select difficulty level:");
                foreach (MenuAction menuAction in difficultyLevelMenu)
                {
                    Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
                }

                operation = Console.ReadKey();
                Int32.TryParse(operation.KeyChar.ToString(), out diffLvl);
            }
            Console.WriteLine();

            return diffLvl;
        }

        public bool IsHeroDead(Hero hero)
        
        {
            if (hero.Health <= 0)
            {
                Console.WriteLine("You are dead!");
                Console.WriteLine("Better luck next time!");
                throw new MyException("You lost");
            }
            return false;
        }

        public void PrintTheStats(IPerson person1, IPerson person2)
        {
            Console.WriteLine("");
            person1.PrintStats();
            Console.WriteLine("");
            person2.PrintStats();
            Console.WriteLine("");
        }

        public int ChoiceAction()
        {
            Console.WriteLine("What would you like to do?");
            var fightMenu = _actionService.GetMenuActionsByMenuName("Battle");
            foreach (MenuAction menuAction in fightMenu)
            {
                Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
            }

            var operation = Console.ReadKey();
            int battle;
            Int32.TryParse(operation.KeyChar.ToString(), out battle);

            while (battle < 1 || battle > fightMenu.OrderBy(o => o.Id).LastOrDefault().Id)
            {
                Console.WriteLine("\nBad battle attack");
                Console.WriteLine("Please select battle attack:");
                foreach (MenuAction menuAction in fightMenu)
                {
                    Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
                }

                operation = Console.ReadKey();
                Int32.TryParse(operation.KeyChar.ToString(), out battle);
            }

            if (battle == 3)
            {
                Console.WriteLine(". Please select special attack:");
                fightMenu = _actionService.GetMenuActionsByMenuName("SpecialAttack");
                foreach (MenuAction menuAction in fightMenu)
                {
                    Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
                }

                operation = Console.ReadKey();
                Int32.TryParse(operation.KeyChar.ToString(), out battle);

                while (battle < 4 || battle > fightMenu.OrderBy(o => o.Id).LastOrDefault().Id)
                {
                    Console.WriteLine("\nBad special attack");
                    Console.WriteLine("3. Please select special attack:");
                    foreach (MenuAction menuAction in fightMenu)
                    {
                        Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
                    }

                    operation = Console.ReadKey();
                    Int32.TryParse(operation.KeyChar.ToString(), out battle);
                }
            }
            Console.WriteLine();
            return battle;
        }

        public void Fight(Hero hero, Enemy enemy)
        {
            while (enemy.Health > 0 && hero.Health > 0)
            {
                PrintTheStats(enemy, hero);
                Random rnd = new Random();

                hero.HeroTurn(ChoiceAction(), enemy);

                if (enemy.Health > 0)
                {
                    enemy.EnemyTurn(rnd.Next(1, 3), hero);
                }
            }
        }

        public void BattleWithArcher(Hero hero, Archer archer)
        {
            if (IsHeroDead(hero))
                return;
            else
                Fight(hero, archer);

            if (IsHeroDead(hero))
                return;

            Console.WriteLine($"You killed {archer.Name}");
            Console.WriteLine("Gained +20 exp");
            hero.GainExp(20);
            if (hero.Exp >= hero.RequiredExp)
                hero.LevelUp();

            Console.ReadLine();
        }

        public void BattleWithKnight(Hero hero, Knight knight)
        {
            if (IsHeroDead(hero))
                return;
            else
                Fight(hero, knight);

            if (IsHeroDead(hero))
                return;

            Console.WriteLine($"You killed {knight.Name}");
            Console.WriteLine("Gained +30 exp");
            hero.GainExp(30);
            if (hero.Exp >= hero.RequiredExp)
                hero.LevelUp();

            Console.ReadLine();
        }

        public void BattleWithDragon(Hero hero, Dragon dragon)
        {
            if (IsHeroDead(hero))
                return;
            else
                Fight(hero, dragon);

            if (IsHeroDead(hero))
                return;

            Console.WriteLine($"You killed {dragon.Name}");
            Console.WriteLine("Gained +50 exp");
            hero.GainExp(50);
            if (hero.Exp >= hero.RequiredExp)
                hero.LevelUp();

            Console.ReadLine();
            //Console.Clear();
        }

        public void Begin(int numberOfEnemies)
        {
            Console.WriteLine("You are the best hero who is on the way to kill the dragon");
            Console.WriteLine("Dragon wants to destroy your kingdom");
            Console.WriteLine($"On your way to the lairs of the dragons, there is couple of archers ({numberOfEnemies}).");
            Console.WriteLine("And they dont want to let you go without a fight");
            Console.ReadLine();
        }

        public void BeforeKnights(int numberOfEnemies)
        {
            Console.WriteLine("Archers dont want to duel you. Well Done! You continue on to the dragons lair!");
            Console.WriteLine("However the dragons hired some knights to defense their lairs from people like you");
            Console.WriteLine("Watch out!");
            Console.WriteLine($"Theres {numberOfEnemies} of them that have found out about your quest.");
            Console.ReadLine();
        }

        public void BeforeDragons(int numberOfEnemies)
        {
            Console.WriteLine("Congrats you killed them, you continue on your journey!");
            Console.WriteLine("You are close to the dragon lairs.");
            Console.WriteLine("It's hot and dangerous in there.");
            Console.WriteLine($"There is {numberOfEnemies} dragons.");
            Console.WriteLine("The time has come to end the dragons rampage!");
            Console.ReadLine();
        }

        public void TheEnd()
        {
            Console.WriteLine("You killed the dragons and saved the kingdom!");
            Console.WriteLine("Congrats!");
        }

        public void UpgradeEnemies(int upgrade)
        {
            _enemyService.UpgradeEnemies(upgrade);
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


