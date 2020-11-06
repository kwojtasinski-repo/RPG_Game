using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Common;
using System.Collections.Generic;


namespace RPG_GAME.Service.Concrete
{
    public class MenuActionService : BaseService<MenuAction>
    {
        public MenuActionService()
        {
            Initialize();
        }

        public List<MenuAction> GetMenuActionsByMenuName(string menuName)
        {
            List<MenuAction> result = new List<MenuAction>();
            foreach (var menuAction in Objects)
            {
                if (menuAction.MenuName == menuName)
                {
                    result.Add(menuAction);
                }
            }
            return result;
        }

        private void Initialize()
        {
            AddObject(new MenuAction(1, "Create Hero", "Main"));
            AddObject(new MenuAction(2, "Remove Hero", "Main"));
            AddObject(new MenuAction(3, "Show details of Hero", "Main"));
            AddObject(new MenuAction(4, "Show your Heroes", "Main"));
            AddObject(new MenuAction(5, "Start Game", "Main"));
            AddObject(new MenuAction(6, "Exit Game", "Main"));

            AddObject(new MenuAction(1, "Warrior", "CreateHero"));
            AddObject(new MenuAction(2, "Paladin", "CreateHero"));

            AddObject(new MenuAction(1, "Easy Level", "DifficultyLevel"));
            AddObject(new MenuAction(2, "Medium Level", "DifficultyLevel"));
            AddObject(new MenuAction(3, "Hard Level", "DifficultyLevel"));

            AddObject(new MenuAction(1, "Attack", "Battle"));
            AddObject(new MenuAction(2, "Heal", "Battle"));
            AddObject(new MenuAction(3, "Special attack", "Battle"));

            AddObject(new MenuAction(4, "Spin Attack", "SpecialAttack"));
            AddObject(new MenuAction(5, "Double Slash", "SpecialAttack"));
        }
    }
}
