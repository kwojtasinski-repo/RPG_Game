using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Abstract;
using RPG_GAME.Service.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.Service.Managers
{
    public class HeroManager
    {
        private readonly MenuActionService _actionService;
        private IService<Hero> _heroService;

        public HeroManager(MenuActionService actionService, IService<Hero> heroService)
        {
            _actionService = actionService;
            _heroService = heroService;
        }

        public int AddNewHero()
        { 
            var addNewHeroMenu = _actionService.GetMenuActionsByMenuName("CreateHero");
            int typeId;
            int lastTypeId = addNewHeroMenu.OrderBy(o => o.Id).LastOrDefault().Id;
            int lastId = _heroService.GetLastId();
            Console.WriteLine("\nYou are current in menu \"Add Hero\". If you want go back press -1 else press enter");
            int goBack = EnterValue(Console.ReadLine());
            if (goBack == -1)
            {
                return 0;
            }

            typeId = ReturnProperEnteredTypeId();
            string name = ReturnProperEnteredName(typeId);

            Console.WriteLine($"\nYou can cancel adding Hero {name} {addNewHeroMenu[typeId - 1].Name} by pressing -1");
            goBack = EnterValue(Console.ReadLine());
            if (goBack == -1)
            {
                Console.WriteLine("You canceled adding Hero");
                return 0;
            }

            var hero = new Hero(lastId + 1, name, GetHeroInitialStats(addNewHeroMenu[typeId - 1].Name,
                "health"), GetHeroInitialStats(addNewHeroMenu[typeId - 1].Name, "attack"), addNewHeroMenu[typeId - 1].Name,
                typeId);
            _heroService.AddObject(hero);
            return hero.Id;
        }

        public int AddNewHero(string name, int typeId)
        {
            var addNewHeroMenu = _actionService.GetMenuActionsByMenuName("CreateHero");
            int lastTypeId = addNewHeroMenu.OrderBy(o => o.Id).LastOrDefault().Id;
            bool checkHeroName = CheckIfHeroHasTheSameName(name);

            if (typeId < 1 || typeId > lastTypeId)
            {
                return 0;
            }

            if(checkHeroName || !HeroHasAtLeastThreeCharacters(name))
            {
                return 0;
            }
            else
            {
                int lastId = _heroService.GetLastId();
                var hero = new Hero(lastId + 1, name, GetHeroInitialStats(addNewHeroMenu[typeId - 1].Name,
                    "health"), GetHeroInitialStats(addNewHeroMenu[typeId - 1].Name, "attack"), addNewHeroMenu[typeId - 1].Name,
                    typeId);
                _heroService.AddObject(hero);
                return hero.Id;
            }
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
            int id = EnterValue(Console.ReadKey().KeyChar.ToString());
            if (_heroService.Objects.Count > 0)
                _heroService.RemoveObject(_heroService.GetObjectById(id));
            else
                Console.WriteLine($"\nThere is no Hereo with id {id}");

            return id;
        }

        public void RemoveHeroById(int id)
        {
            var hero = _heroService.GetObjectById(id);
            _heroService.RemoveObject(hero);
        }

        public void HeroDetails()
        {
            Console.WriteLine("\nPlease enter id for hero you want to show or press Enter to cancel");
            int id = EnterValue(Console.ReadLine());
            HeroNameCanBeChanged(id);
        }

        public List<Hero> ShowHeroes()
        {
            int typeId;
            var chooseMenu = _actionService.GetMenuActionsByMenuName("CreateHero");
            int lastTypeId = chooseMenu.OrderBy(o => o.Id).LastOrDefault().Id;

            typeId = ReturnProperEnteredTypeId();
            Console.WriteLine();

            var heroes = _heroService.GetAllObjects();
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
            int chosenHero = EnterValue(Console.ReadLine());
            return chosenHero;
        }

        public bool CheckIfHeroHasTheSameName(string name)
        {
            var sortedHeroes = GetAllHeroes().OrderBy(o => o.Name).ToList();
            
            foreach (var hero in sortedHeroes)
            {
                if (hero.Name.ToUpper().Replace(" ", "") == name.ToUpper().Replace(" ", ""))
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool HeroHasAtLeastThreeCharacters(string name)
        {
            if (name.Replace(" ", "").Count() < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int ChangeHeroName(Hero hero, string name)
        {
            bool checkedName = CheckIfHeroHasTheSameName(name);
            int heroNameChangedId = 0;

            if (!checkedName && HeroHasAtLeastThreeCharacters(name)) 
            {
                hero.Name = name;
                heroNameChangedId = hero.Id;
            }

            return heroNameChangedId;
        }

        private int EnterValue(string value)
        {
            Int32.TryParse(value, out int typedValue);
            return typedValue;
        }

        private int ReturnProperEnteredTypeId()
        {
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
                typeId = EnterValue(Console.ReadKey().KeyChar.ToString());
                Console.WriteLine();
                if (typeId < 1 || typeId > lastTypeId)
                {
                    Console.WriteLine("\nIncorrect Type Id");
                }
            }
            while (typeId < 1 || typeId > lastTypeId);

            return typeId;
        }

        private string ReturnProperEnteredName(int typeId)
        {
            string name;
            var addNewHeroMenu = _actionService.GetMenuActionsByMenuName("CreateHero");
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
                else if (!HeroHasAtLeastThreeCharacters(name))
                {
                    Console.WriteLine($"You entered a name too short for the {addNewHeroMenu[typeId - 1].Name}, name should have at least 3 characters (not include spaces).");
                }
            }
            while (!HeroHasAtLeastThreeCharacters(name) || checkHeroName);

            return name;
        }

        private void HeroNameCanBeChanged(int id)
        {
            var hero = _heroService.GetObjectById(id);
            if (hero != null)
            {
                hero.PrintStats();
                Console.WriteLine("You can change name your hero if you want to continue press 1 (default is No)");
                int choose = EnterValue(Console.ReadKey().KeyChar.ToString());

                if (choose == 1)
                {
                    CheckIfHeroNameWasChangedProperly(hero);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Hero doesn't exist");
            }
        }

        private void CheckIfHeroNameWasChangedProperly(Hero hero)
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
    }
}
