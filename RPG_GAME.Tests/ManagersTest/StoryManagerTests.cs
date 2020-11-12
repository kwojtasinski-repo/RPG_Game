using FluentAssertions;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Concrete;
using RPG_GAME.Service.Managers;
using Xunit;

namespace RPG_GAME.Tests.ManagersTest
{
    public class StoryManagerTests
    {
        [Fact] 
        public void CheckIsHeroDead()
        {
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "HeroDead", 10, 15, "Warrior", 1);
            StoryManager storyManager = new StoryManager(new MenuActionService(), hero);

            var returnedInformation = storyManager.IsHeroDead(hero);
            hero2.Health = 0;
            void act() => storyManager.IsHeroDead(hero2);
            var ex = Record.Exception(act);

            returnedInformation.Should().BeFalse();
            Assert.NotNull(ex);
            Assert.IsType<MyException>(ex);
        }
    }
}
