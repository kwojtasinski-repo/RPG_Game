using FluentAssertions;
using Moq;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Abstract;
using RPG_GAME.Service.Concrete;
using RPG_GAME.Service.Managers;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.UnitTests.ManagersTest
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
        public void CantReturnHeroWithBadId()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            var mock = new Mock<IService<Hero>>(); // tworzenie symulacji serwisu
            mock.Setup(s => s.GetObjectById(1)).Returns(hero);

            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returnedHero = manager.GetHeroById(2);

            //Assert
            returnedHero.Should().BeNull();
            returnedHero.Should().NotBeSameAs(hero);
        }

        [Fact] 
        public void CanDeleteHeroWithBadId()
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
        public void CantDeleteHeroWithIncorrectId()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Slaughter2", 10, 15, "Warrior", 1);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetObjectById(1)).Returns(hero);
            mock.Setup(m => m.RemoveObject(It.IsAny<Hero>())); // jakikolwiek parametr typu Hero
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            manager.RemoveHeroById(hero.Id);

            //Assert
            mock.Verify(m => m.RemoveObject(hero2), Times.Never); // sprawdz czy metoda zostala wywolana z parametrem hero
        }

        [Fact] 
        public void CanAddHero()
        {
            //Arrange
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>());
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returnedWarriorId = manager.AddNewHero("Slaughter", 1);

            //Assert
            returnedWarriorId.Should().NotBe(0);
            returnedWarriorId.Should().Be(1);
        }

        [Fact]
        public void CantAddHeroWithIncorrectParameters()
        {
            //Arrange
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>());
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returnedWrongTypeId = manager.AddNewHero("Slaughter", 3);
            var returnedTooSmallName = manager.AddNewHero("sw", 2);
            var returnedNameWithSpaces = manager.AddNewHero("   ", 1);

            //Assert
            returnedWrongTypeId.Should().Be(0);
            returnedTooSmallName.Should().Be(0);
            returnedNameWithSpaces.Should().Be(0);
        }

        [Fact]
        public void CantAddSameHero()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero });
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returnHeroWithSameName = manager.AddNewHero("Slaughter", 1);
            var returnHeroWithSameName2 = manager.AddNewHero("Slaughter", 2);

            //Assert
            returnHeroWithSameName.Should().Be(0);
            returnHeroWithSameName2.Should().Be(0);
        }

        [Fact] 
        public void HeroesHasSameNames()
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
        public void HeroDoesntHaveSameName()
        {
            //Arrange
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "MrSlaughter", 14, 11, "Paladin", 2);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero });
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            //Act
            var returned = manager.CheckIfHeroHasTheSameName(hero2.Name);

            //Assert
            returned.Should().BeFalse();
            returned.Should().NotBe(true);
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
        public void CanAddHeroInitialStatsWithProperParameters()
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
        public void CantAddHeroInitialStatsWithIncorrectParameters()
        {
            HeroService heroService = new HeroService();
            var manager = new HeroManager(new MenuActionService(), heroService);
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Hero", 14, 11, "Paladin", 1);

            var returnInitialWarriorHealth = manager.GetHeroInitialStats(hero.Profession, "Hhealth");
            var returnInitialWarriorAttack = manager.GetHeroInitialStats(hero.Profession, "Aatack");
            var returnInitialPaladinHealth = manager.GetHeroInitialStats(hero2.Profession, "Hhealth");
            var returnInitialPaladinAttack = manager.GetHeroInitialStats(hero2.Profession, "Aatack");

            returnInitialWarriorAttack.Should().Be(0);
            returnInitialWarriorAttack.Should().NotBe(hero.Attack);

            returnInitialWarriorHealth.Should().Be(0);
            returnInitialWarriorHealth.Should().NotBe(hero.Health);

            returnInitialPaladinAttack.Should().Be(0);
            returnInitialPaladinAttack.Should().NotBe(hero2.Attack);

            returnInitialPaladinHealth.Should().Be(0);
            returnInitialPaladinHealth.Should().NotBe(hero2.Health);
        }

        [Fact] 
        public void CanChangeHeroName()
        {
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Hero", 14, 11, "Paladin", 1);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero, hero2 });
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            var returnHero2IdAfterChange = manager.ChangeHeroName(hero2, "Hero Slaughter");

            returnHero2IdAfterChange.Should().NotBe(0);
            returnHero2IdAfterChange.Should().Be(hero2.Id);
        }

        [Fact]
        public void CantChangeHeroName()
        {
            Hero hero = new Hero(1, "Slaughter", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Hero", 14, 11, "Paladin", 1);
            var mock = new Mock<IService<Hero>>();
            mock.Setup(m => m.GetAllObjects()).Returns(new List<Hero>() { hero, hero2 });
            var manager = new HeroManager(new MenuActionService(), mock.Object);

            var returnHero2IdAfterChangeWithOnlySpace = manager.ChangeHeroName(hero2, "              ");
            var returnHero2IdAfterChangeWithTwoCharacters = manager.ChangeHeroName(hero2, "RS");
            var returnHero2IdAfterChangeWithSameName = manager.ChangeHeroName(hero2, "Slaughter");
            var returnHero2IdAfterChangeWithSameNameUpperCase = manager.ChangeHeroName(hero2, "SLAUGHTER");
            var returnHero2IdAfterChangeWithSameNameUpperCaseAndSpaces = manager.ChangeHeroName(hero2, " S L A U G H T E R ");
            var returnHero2IdAfterChangeWithSameNameLowCaseAndSpaces = manager.ChangeHeroName(hero2, " s l a u g h t e r ");

            returnHero2IdAfterChangeWithOnlySpace.Should().NotBe(hero2.Id);
            returnHero2IdAfterChangeWithOnlySpace.Should().Be(0);

            returnHero2IdAfterChangeWithTwoCharacters.Should().NotBe(hero2.Id);
            returnHero2IdAfterChangeWithTwoCharacters.Should().Be(0);

            returnHero2IdAfterChangeWithSameName.Should().NotBe(hero2.Id);
            returnHero2IdAfterChangeWithOnlySpace.Should().Be(0);

            returnHero2IdAfterChangeWithSameNameUpperCase.Should().NotBe(hero2.Id);
            returnHero2IdAfterChangeWithSameNameUpperCase.Should().Be(0);

            returnHero2IdAfterChangeWithSameNameUpperCaseAndSpaces.Should().NotBe(hero2.Id);
            returnHero2IdAfterChangeWithSameNameUpperCaseAndSpaces.Should().Be(0);

            returnHero2IdAfterChangeWithSameNameLowCaseAndSpaces.Should().NotBe(hero2.Id);
            returnHero2IdAfterChangeWithSameNameLowCaseAndSpaces.Should().Be(0);
        }
    }
}
