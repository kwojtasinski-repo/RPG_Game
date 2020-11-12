using FluentAssertions;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Concrete;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.Tests.ServiceTest
{
    public class MenuActionServiceTests
    {
        [Fact]
        public void CanGetMenuByCategory()
        {
            var menuActionService = new MenuActionService();

            var mainMenu = menuActionService.GetMenuActionsByMenuName("Main");

            mainMenu.Should().NotBeEmpty();
            mainMenu.Should().NotBeNull();
            mainMenu.Should().BeOfType(typeof(List<MenuAction>));
        }
    }
}
