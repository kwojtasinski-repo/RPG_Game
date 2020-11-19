using FluentAssertions;
using Moq;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Concrete;
using RPG_GAME.Service.Managers;
using Xunit;

namespace RPG_GAME.Tests.ManagersTest
{
    public class StoryManagerTests
    {
        [Fact] 
        public void HeroIsntDead()
        {
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            var mock = new Mock<EnemyService>();
            StoryManager storyManager = new StoryManager(new MenuActionService(), hero, mock.Object);

            hero.Health = 1;
            var returnedInformation = storyManager.IsHeroDead(hero);

            returnedInformation.Should().BeFalse();
        }

        [Fact]
        public void HeroIsDead()
        {
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            var mock = new Mock<EnemyService>();
            StoryManager storyManager = new StoryManager(new MenuActionService(), hero, mock.Object);

            hero.Health = 0;
            void act() => storyManager.IsHeroDead(hero);
            var ex = Record.Exception(act);

            Assert.NotNull(ex);
            Assert.IsType<MyException>(ex);
        }
    }
}
