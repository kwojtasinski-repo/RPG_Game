using RPG_GAME.Service.Concrete;
using RPG_GAME.Service.Managers;
using System;
using RPG_GAME.Helpers;
using RPG_GAME.Core.Entity;

namespace RPG_GAME
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuActionService menuActionService = new MenuActionService();
            HeroManager heroManager = new HeroManager(menuActionService);
            StoryManager storyManager;
            int tryHard = 0;

            Console.WriteLine("Welcome to RPG Game app!");
            while (true)
            {
                int exit = 0;
                Console.WriteLine("Please let me know what you want to do:");
                var mainMenu = menuActionService.GetMenuActionsByMenuName("Main");
                for (int i = 0; i < mainMenu.Count; i++)
                {
                    Console.WriteLine($"{mainMenu[i].Id}. {mainMenu[i].Name}");
                }

                var operation = Console.ReadKey();

                switch (operation.KeyChar)
                {
                    case '1': 
                        var newId = heroManager.AddNewHero();
                        break;
                    case '2':
                        heroManager.RemoveHero();
                        break;
                    case '3': 
                        heroManager.HeroDetails();
                        break;
                    case '4':
                        var toShow = heroManager.ShowHeroes();
                        Console.WriteLine(toShow.ToStringTable(new[] { "Id", "Name", "Health", "Max Health",
                            "Attack", "Heal Level", "Experience", "Level", "Required Experience",
                            "Profession", "Typ Id" }, a => a.Id, a => a.Name, a => a.Health, a => a.MaxHealth,
                            a => a.Attack, a => a.HealLvl, a => a.Exp, a => a.Level, a => a.RequiredExp,
                            a => a.Profession, a => a.TypeId));
                        break;
                    case '5':
                        var heroes = heroManager.GetAllHeroes();
                        if (heroes.Count > 0)
                        {
                            Console.WriteLine();
                            Console.WriteLine(heroes.ToStringTable(new[] { "Id", "Name", "Health", "Max Health",
                            "Attack", "Heal Level", "Experience", "Level", "Required Experience",
                            "Profession", "Typ Id" }, a => a.Id, a => a.Name, a => a.Health, a => a.MaxHealth,
                            a => a.Attack, a => a.HealLvl, a => a.Exp, a => a.Level, a => a.RequiredExp,
                            a => a.Profession, a => a.TypeId));
                            int chosenHero = -1;

                            Hero hero = null;
                            while (hero == null)
                            {
                                chosenHero = heroManager.SelectCharacter();
                                
                                if (chosenHero == -1)
                                    break;

                                hero = heroManager.GetHeroById(chosenHero);
                            }

                            if (chosenHero != -1)
                            {
                                Console.WriteLine($"You chose Hero {hero.Name}, level:{hero.Level}, profession:{hero.Profession}");
                                storyManager = new StoryManager(menuActionService, hero);
                                storyManager.UpgradeEnemies(tryHard);
                                storyManager.Start();
                                hero.Reset();
                                tryHard++;
                            }
                        }
                        else
                            Console.WriteLine("\nThere is no heroes to choose");
                        break;
                    case '6':
                        exit = 1;
                        break;
                    default:
                        Console.WriteLine("Action you entered does not exist");
                        break;
                }

                if (exit == 1)
                    break;
                
            }
        }

    }
}
