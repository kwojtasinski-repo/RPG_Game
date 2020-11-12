using FluentAssertions;
using Moq;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Abstract;
using RPG_GAME.Service.Concrete;
using RPG_GAME.Service.Managers;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.Tests.ManagersTest
{
    public class HeroManagerTests
    {
        [Fact]
        public void CanReturnHeroWithProperId()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            var mock = new Mock<IService<Hero>>(); // tworzenie symulacji serwisu
            mock.Setup(s => s.GetObjectById(1)).Returns(hero); 

            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returnedHero = manager.GetHeroById(hero.Id);

            //Assert
            returnedHero.Should().BeOfType(typeof(Hero));
            returnedHero.Should().NotBeNull();
            returnedHero.Should().BeSameAs(hero);
        }

        [Fact] 
        public void CanDeleteHeroWithProperId()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetObjectById(1)).Returns(hero);
            mock.Setup(m => m.RemoveObject(It.IsAny<Hero>())); // jakikolwiek parametr typu Hero
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            manager.RemoveHeroById(hero.Id);

            //Assert
            mock.Verify(m => m.RemoveObject(hero)); // sprawdz czy metoda zostala wywolana z parametrem hero
        }

        [Fact] 
        public void CanAddHero()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            var heroService = new HeroService();
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>());
            var manager = new HeroManager(new MenuActionService(), mock.Object);
            var mock2 = new Mock<IService<Hero>>();
            mock2.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero });
            var manager2 = new HeroManager(new MenuActionService(), mock2.Object);

            //Act
            var returnedWarriorId = manager.AddNewHero("Slaughter", 1);
            var returnedWrongTypeId = manager.AddNewHero("Slaughter", 3);
            var returnedTooSmallName = manager.AddNewHero("sw", 2);
            var returnedNameWithSpaces = manager.AddNewHero("   ", 1);
            var returnHeroWithSameName = manager2.AddNewHero("Slaughter", 1);
            var returnHeroWithSameName2 = manager2.AddNewHero("Slaughter", 2);

            //Assert
            returnedWarriorId.Should().NotBe(0);
            returnedWarriorId.Should().Be(hero.Id);
            returnedWrongTypeId.Should().Be(0);
            returnedTooSmallName.Should().Be(0);
            returnedNameWithSpaces.Should().Be(0);
            returnHeroWithSameName.Should().Be(0);
            returnHeroWithSameName2.Should().Be(0);
        }

        [Fact] 
        public void CheckIfHeroesHasSameNames()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Slaughter", 14, 11, "Paladin", 2);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero });
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returned = manager.CheckIfHeroHasTheSameName(hero2.Name);

            //Assert
            returned.Should().NotBe(false);
            returned.Should().BeTrue();
        }

        [Fact] 
        public void CheckHeroId()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Hero", 10, 15, "Paladin", 1);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero });
            mock.Setup(m => m.GetLastId()).Returns(hero.Id);
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returnedId = manager.AddNewHero(hero2.Name, hero2.TypeId);

            //Assert
            returnedId.Should().NotBe(null);
            returnedId.Should().NotBe(hero.Id);
            returnedId.Should().Be(hero2.Id);
        }

        [Fact] 
        public void CanAddHeroInitialStats()
        {
            HeroService heroService = new HeroService();
            var manager = new HeroManager(new MenuActionService(), heroService);
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Hero", 14, 11, "Paladin", 1);

            var returnInitialWarriorHealth = manager.GetHeroInitialStats(hero.Profession, "health");
            var returnInitialWarriorAttack = manager.GetHeroInitialStats(hero.Profession, "attack");
            var returnInitialPaladinHealth = manager.GetHeroInitialStats(hero2.Profession, "health");
            var returnInitialPaladinAttack = manager.GetHeroInitialStats(hero2.Profession, "attack");

            returnInitialWarriorAttack.Should().NotBe(0);
            returnInitialWarriorAttack.Should().Be(hero.Attack);
            returnInitialWarriorHealth.Should().NotBe(0);
            returnInitialWarriorHealth.Should().Be(hero.Health);
            returnInitialPaladinAttack.Should().NotBe(0);
            returnInitialPaladinAttack.Should().Be(hero2.Attack);
            returnInitialPaladinHealth.Should().NotBe(0);
            returnInitialPaladinHealth.Should().Be(hero2.Health);
        }
        
        [Fact] 
        public void CanChangeHeroName()
        {
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Hero", 14, 11, "Paladin", 1);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero, hero2 });
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            var returnHero2Id = manager.ChangeHeroName(hero2, "Slaughter");
            var returnHero2IdAfterChange = manager.ChangeHeroName(hero2, "Hero Slaughter");

            returnHero2Id.Should().NotBe(hero2.Id);
            returnHero2Id.Should().Be(0);
            returnHero2IdAfterChange.Should().NotBe(0);
            returnHero2IdAfterChange.Should().Be(hero2.Id);
        }
        
    }
}
