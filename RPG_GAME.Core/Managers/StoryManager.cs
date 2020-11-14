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
        public bool ChoseSameLvl { get; private set; }
        public int DiffLvl { get; private set; }

        public StoryManager(MenuActionService actionService, Hero hero)
        {
            _actionService = actionService;
            _hero = hero;
            enemies = new List<Enemy>();
            _enemyService = new EnemyService();
        }

        public void Start()
        {
            int diffLvlBefore = DiffLvl;
            DiffLvl = ChooseDifficultyLevel();
            if (DiffLvl == diffLvlBefore)
            {
                ChoseSameLvl = true;
            }
            else
            {
                ChoseSameLvl = false;
            }
            enemies = _enemyService.GetEnemiesByDiffLvl(DiffLvl);
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
            catch (Exception e)
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
            int diffLvl;
            int diffLvlMax = difficultyLevelMenu.OrderBy(o => o.Id).LastOrDefault().Id;

            do
            {
                Console.WriteLine("\nPlease select difficulty level:");
                ShowMenuAction(difficultyLevelMenu);

                diffLvl = EnterValue(Console.ReadKey().KeyChar.ToString());
                Console.WriteLine();

                if(diffLvl < 1 || diffLvl > diffLvlMax)
                {
                    Console.WriteLine("\nBad Type difficulty level");
                }
            }
            while (diffLvl < 1 || diffLvl > diffLvlMax);

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
            int battle = ReturnProperEnteredBattleId();
            return battle;
        }

        private int ReturnProperEnteredBattleId()
        {
            var fightMenu = _actionService.GetMenuActionsByMenuName("Battle");
            int battle;
            int lastBattleId = fightMenu.OrderBy(o => o.Id).LastOrDefault().Id;

            do
            {
                Console.WriteLine("Please select battle attack:");
                ShowMenuAction(fightMenu);

                battle = EnterValue(Console.ReadKey().KeyChar.ToString());
                Console.WriteLine();

                if (battle < 1 || battle > lastBattleId)
                {
                    Console.WriteLine("Bad battle attack");
                }
            }
            while (battle < 1 || battle > lastBattleId);

            if (battle == 3)
            {
                fightMenu = _actionService.GetMenuActionsByMenuName("SpecialAttack");
                lastBattleId = fightMenu.OrderBy(o => o.Id).LastOrDefault().Id;
                do
                {
                    Console.WriteLine("\n3. Please select special attack:");
                    ShowMenuAction(fightMenu);

                    battle = EnterValue(Console.ReadKey().KeyChar.ToString());

                    if (battle < 4 || battle > lastBattleId)
                    {
                        Console.WriteLine("\nBad special attack");
                    }
                       
                }
                while (battle < 4 || battle > lastBattleId);           
            }
            Console.WriteLine();

            return battle;
        }

        private void ShowMenuAction(List<MenuAction> menuAction)
        {
            foreach (MenuAction action in menuAction)
            {
                Console.WriteLine($"{action.Id}. {action.Name}");
            }
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

        public void UpgradeEnemiesByDiffLvl(int upgrade, int diffLvl)
        {
            _enemyService.UpgradeEnemiesByDiffLvl(upgrade, diffLvl);
        }

        private int EnterValue(string value)
        {
            Int32.TryParse(value, out int typedValue);
            return typedValue;
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


