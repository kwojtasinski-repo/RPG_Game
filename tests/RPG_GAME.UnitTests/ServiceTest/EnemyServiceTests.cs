using FluentAssertions;
using RPG_GAME.Core.Entity;
using RPG_GAME.Service.Concrete;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RPG_GAME.UnitTests.ServiceTest
{
    public class EnemyServiceTests
    {
        [Fact]
        public void CanGetEnemiesByDiffLvl()
        {
            var enemyService = new EnemyService();

            var enemiesByDiffLvl = enemyService.GetEnemiesByDiffLvl(1);

            enemiesByDiffLvl.Should().NotBeNull();
            enemiesByDiffLvl.Should().NotBeEmpty();
            enemiesByDiffLvl.Should().BeOfType(typeof(List<Enemy>));
        }

        [Fact]
        public void CantGetEnemiesByIncorrectDiffLvl()
        {
            var enemyService = new EnemyService();

            var enemiesByDiffLvl = enemyService.GetEnemiesByDiffLvl(4);

            enemiesByDiffLvl.Should().BeEmpty();
            enemiesByDiffLvl.Should().BeOfType(typeof(List<Enemy>));
        }

        [Fact]
        public void CanGetEnemiesByCategory()
        {
            var enemyService = new EnemyService();

            var enemiesByCategory = enemyService.FindEnemiesByCategory(enemyService.Objects, "Archer");

            enemiesByCategory.Should().NotBeNull();
            enemiesByCategory.Should().NotBeEmpty();
            enemiesByCategory.Should().BeOfType(typeof(List<Enemy>));
        }

        [Fact]
        public void CantGetEnemiesByIncorrectCategory()
        {
            var enemyService = new EnemyService();

            var enemiesByCategory = enemyService.FindEnemiesByCategory(enemyService.Objects, "Master");

            enemiesByCategory.Should().BeEmpty();
            enemiesByCategory.Should().BeOfType(typeof(List<Enemy>));
        }

        [Fact]
        public void CanResetEnemies()
        {
            var enemyService = new EnemyService();
            var enemyServiceActual = new EnemyService();

            var archers = enemyService.FindEnemiesByCategory(enemyService.Objects, "Archer");
            var archer = archers.FirstOrDefault(en => en.Attack == 7);
            archer.SetHealth(6);
            enemyService.ResetEnemies();

            archers.Should().NotBeNull();
            archers.Should().NotBeEmpty();
            archers.Should().BeOfType(typeof(List<Enemy>));
            archers.Should().BeEquivalentTo(enemyServiceActual.FindEnemiesByCategory(enemyServiceActual.Objects, "Archer"));
        }
    }
}
