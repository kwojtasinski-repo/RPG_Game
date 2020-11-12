using FluentAssertions;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Common;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.Tests.ServiceTest
{
    public class BaseServiceTests
    {
        [Fact]
        public void CanAddObject()
        {
            Hero hero = new Hero(1, "Heros", 10, 15, "Warrior", 1);
            var baseService = new BaseService<Hero>();

            var returnedId = baseService.AddObject(hero);

            returnedId.Should().NotBe(0);
            returnedId.Should().Be(hero.Id);
        }

        [Fact]
        public void CanGetAllObjects()
        {
            Hero hero = new Hero(1, "Heros", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(2, "Nick", 10, 15, "Paladin", 2);
            Hero hero3 = new Hero(3, "Name", 10, 15, "Warrior", 1);

            var baseService = new BaseService<Hero>();

            baseService.AddObject(hero);
            baseService.AddObject(hero2);
            baseService.AddObject(hero3);
            var heroesList = baseService.GetAllObjects();

            heroesList.Should().NotBeNull();
            heroesList.Should().NotBeEmpty();
            heroesList.Should().BeOfType(typeof(List<Hero>));
            heroesList.Should().HaveCount(3);
            heroesList.Should().BeEquivalentTo(new List<Hero> { hero, hero2, hero3 });
        }


        [Fact]
        public void CanGetObjectById()
        {
            Hero hero = new Hero(1, "Heros", 10, 15, "Warrior", 1);

            var baseService = new BaseService<Hero>();

            baseService.AddObject(hero);
            var heroFromBaseService = baseService.GetObjectById(1);

            heroFromBaseService.Should().NotBeNull();
            heroFromBaseService.Should().BeOfType(typeof(Hero));
            heroFromBaseService.Should().BeSameAs(hero);
        }

        [Fact]
        public void CanGetLastId()
        {
            Hero hero = new Hero(1, "Heros", 10, 15, "Warrior", 1);
            Hero hero2 = new Hero(3, "Nick", 10, 15, "Paladin", 2);
            Hero hero3 = new Hero(2, "Name", 10, 15, "Warrior", 1);

            var baseService = new BaseService<Hero>();

            baseService.AddObject(hero);
            baseService.AddObject(hero2);
            baseService.AddObject(hero3);
            var lastId = baseService.GetLastId();

            lastId.Should().NotBe(0);
            lastId.Should().BeGreaterThan(2);
            lastId.Should().Be(hero2.Id);
        }
    }
}
