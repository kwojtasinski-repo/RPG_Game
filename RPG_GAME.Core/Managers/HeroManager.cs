using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.Service.Managers
{
    public class HeroManager
    {
        private readonly MenuActionService _actionService;
        private HeroService _heroService;

        public HeroManager(MenuActionService actionService)
        {
            _heroService = new HeroService();
            _actionService = actionService;
        }

        public int AddNewHero()
        {
            var addNewHeroMenu = _actionService.GetMenuActionsByMenuName("CreateHero");
            ConsoleKeyInfo operation;
            int typeId;
            int lastTypeId = addNewHeroMenu.OrderBy(o => o.Id).LastOrDefault().Id;
            int lastId = _heroService.GetLastId();
            Console.WriteLine("\nYou are current in menu \"Add Hero\". If you want go back press -1 else press enter");
            Int32.TryParse(Console.ReadLine(), out int goBack);
            if (goBack == -1)
            {
                return 0;
            }

            do
            {
                Console.WriteLine("\nPlease select Hero:");
                foreach (MenuAction menuAction in addNewHeroMenu)
                {
                    Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
                }

                operation = Console.ReadKey();
                Int32.TryParse(operation.KeyChar.ToString(), out typeId);
                
                if (typeId < 1 || typeId > lastTypeId)
                    Console.WriteLine("\nBad Type Id");
            }
            while (typeId < 1 || typeId > lastTypeId);

            string name;
            bool checkHeroName;

            do
            {
                Console.WriteLine($"\nPlease insert name for {addNewHeroMenu[typeId - 1].Name}:");
                name = Console.ReadLine();
                checkHeroName = CheckIfHeroHasTheSameName(name);

                if (checkHeroName)
                {
                    Console.WriteLine($"This name for {addNewHeroMenu[typeId - 1].Name} already occurs in game.");
                }
                else if (name.Replace(" ", "").Count() < 3)
                {
                    Console.WriteLine($"You entered a name too short for the {addNewHeroMenu[typeId - 1].Name}, name should have at least 3 characters (not include spaces).");
                }
            }
            while (name.Replace(" ", "").Count() < 3 || checkHeroName);

            Console.WriteLine($"\nYou can cancel adding Hero {name} {addNewHeroMenu[typeId - 1].Name} by pressing -1");
            Int32.TryParse(Console.ReadLine(), out goBack);
            if (goBack == -1)
            {
                Console.WriteLine("You canceled adding Hero");
                return 0;
            }

            Hero hero = new Hero(lastId + 1, name, GetHeroInitialStats(addNewHeroMenu[typeId - 1].Name,
                "health"), GetHeroInitialStats(addNewHeroMenu[typeId - 1].Name, "attack"), addNewHeroMenu[typeId - 1].Name,
                typeId);
            _heroService.AddObject(hero);
            return hero.Id;
        }

        public int GetHeroInitialStats(string character, string stat)
        {
            if (character == "Warrior")
            {
                if (stat == "health")
                {
                    return 15;
                }
                else if (stat == "attack")
                {
                    return 10;
                }
                else
                    return 0;
            }
            else if (character == "Paladin")
            {
                if (stat == "health")
                {
                    return 11;
                }
                else if (stat == "attack")
                {
                    return 14;
                }
                else
                    return 0;
            }
            else
                return 0;
        }

        public int RemoveHero()
        {
            Console.WriteLine("\nPlease enter id for hero you want to remove or press Enter to cancel:");
            var itemId = Console.ReadKey();
            Int32.TryParse(itemId.KeyChar.ToString(), out int id);
            if (_heroService.Objects.Count > 0)
                _heroService.RemoveObject(_heroService.GetObjectById(id));
            else
                Console.WriteLine($"\nThere is no Hereo with id {id}");

            return id;
        }

        public void HeroDetails()
        {
            Console.WriteLine("\nPlease enter id for hero you want to show or press Enter to cancel");
            var itemId = Console.ReadLine();
            Int32.TryParse(itemId, out int id);
            var hero = _heroService.GetObjectById(id);
            if (hero != null)
            {
                hero.PrintStats();
                Console.WriteLine("You can change name your hero if you want to continue press 1 (default is No)");
                var keyInput = Console.ReadKey();
                Int32.TryParse(keyInput.KeyChar.ToString(), out int choose);
                if (choose == 1)
                {
                    Console.WriteLine($"\nEnter new name for {hero.Profession}");

                    string name = Console.ReadLine();
                    var changedName = ChangeHeroName(hero, name);

                    if (changedName > 0)
                    {
                        Console.WriteLine("Name has been changed.");
                    }
                    else
                    {
                        Console.WriteLine("Name cannot be changed, name can be the same as the other hero or has less than 3 characters (not include spaces)");
                    }
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine("Hero doesn't exist");
        }

        public List<Hero> ShowHeroes()
        {
            ConsoleKeyInfo heroTypeId;
            int typeId;
            var chooseMenu = _actionService.GetMenuActionsByMenuName("CreateHero");
            int lastTypeId = chooseMenu.OrderBy(o => o.Id).LastOrDefault().Id;

            do
            {
                Console.WriteLine("\nYou can choose");
                foreach (MenuAction menuAction in chooseMenu)
                {
                    Console.WriteLine($"{menuAction.Id}. {menuAction.Name}");
                }
                Console.WriteLine("Please enter Type Id for hero type you want to show:");
                heroTypeId = Console.ReadKey();
                Int32.TryParse(heroTypeId.KeyChar.ToString(), out typeId);
                Console.WriteLine();
                if(typeId < 1 || typeId > lastTypeId)
                {
                    Console.WriteLine("\nIncorrect Type Id");
                }
            }
            while (typeId < 1 || typeId > lastTypeId);
           
            Console.WriteLine();

            List<Hero> heroes = _heroService.GetAllObjects();
            if (heroes.Count < 1)
                Console.WriteLine("There is no Heroes");
            return heroes.Where(item => item.TypeId == typeId).ToList();
        }

        public List<Hero> GetAllHeroes()
        {
            return _heroService.GetAllObjects();
        }

        public Hero GetHeroById(int id)
        {
            var hero = _heroService.GetObjectById(id);
            return hero;
        }

        public int SelectCharacter()
        {
            Console.WriteLine("\nChoose your hero by id or you can leave this option by input -1: ");
            var keyInput = Console.ReadLine();
            Int32.TryParse(keyInput, out int chosenHero);
            return chosenHero;
        }

        public bool CheckIfHeroHasTheSameName(string name)
        {
            List<Hero> sortedHeroes = GetAllHeroes().OrderBy(o => o.Name).ToList();
            
            foreach (Hero hero in sortedHeroes)
            {
                if (hero.Name.ToUpper().Replace(" ", "") == name.ToUpper().Replace(" ", ""))
                {
                    return true;
                }
            }
            
            return false;
        }

        public int ChangeHeroName(Hero hero, string name)
        {
            bool checkedName = CheckIfHeroHasTheSameName(name);
            int heroNameChangedId = 0;

            if (!checkedName && name.Replace(" ", "").Count() > 2) 
            {
                hero.Name = name;
                heroNameChangedId = hero.Id;
            }

            return heroNameChangedId;
        }
    }
}
