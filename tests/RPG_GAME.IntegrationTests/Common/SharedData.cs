using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Common;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Enemies;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RPG_GAME.IntegrationTests.Common
{
    internal static class SharedData
    {
        public static IEnumerable<Enemy> GetEnemies()
        {
            yield return new Enemy(new Guid("00000000-0000-0000-0000-000000000001"), Guid.NewGuid().ToString("N"),
                new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(10, new IncreasingState<int>(2, StrategyIncreasing.ADDITIVE.ToString())),
                new State<decimal>(1000, new IncreasingState<decimal>(100, StrategyIncreasing.ADDITIVE.ToString())),
                Difficulty.EASY.ToString(), Category.Archer.ToString());
            yield return new Enemy(new Guid("00000000-0000-0000-0000-000000000002"), Guid.NewGuid().ToString("N"),
                new State<int>(180, new IncreasingState<int>(40, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(120, new IncreasingState<int>(30, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(15, new IncreasingState<int>(5, StrategyIncreasing.ADDITIVE.ToString())),
                new State<decimal>(3000, new IncreasingState<decimal>(200, StrategyIncreasing.ADDITIVE.ToString())),
                Difficulty.HARD.ToString(), Category.Archer.ToString());
            yield return new Enemy(new Guid("00000000-0000-0000-0000-000000000003"), Guid.NewGuid().ToString("N"),
                new State<int>(300, new IncreasingState<int>(50, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<int>(200, new IncreasingState<int>(30, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<int>(20, new IncreasingState<int>(30, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<decimal>(5000, new IncreasingState<decimal>(20, StrategyIncreasing.PERCENTAGE.ToString())),
                Difficulty.MEDIUM.ToString(), Category.Knight.ToString(),
                new List<SkillEnemy> { new SkillEnemy(Guid.NewGuid(), Guid.NewGuid().ToString("N"), 400, 40, 
                        new IncreasingState<int>(100, StrategyIncreasing.ADDITIVE.ToString()))});
            yield return new Enemy(new Guid("00000000-0000-0000-0000-000000000004"), Guid.NewGuid().ToString("N"),
                new State<int>(500, new IncreasingState<int>(70, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<int>(700, new IncreasingState<int>(60, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<int>(100, new IncreasingState<int>(50, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<decimal>(10000, new IncreasingState<decimal>(30, StrategyIncreasing.PERCENTAGE.ToString())),
                Difficulty.HARD.ToString(), Category.Knight.ToString());
            yield return new Enemy(new Guid("00000000-0000-0000-0000-000000000005"), Guid.NewGuid().ToString("N"),
                new State<int>(1200, new IncreasingState<int>(80, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<int>(1000, new IncreasingState<int>(70, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<int>(500, new IncreasingState<int>(60, StrategyIncreasing.PERCENTAGE.ToString())),
                new State<decimal>(10000, new IncreasingState<decimal>(1000, StrategyIncreasing.PERCENTAGE.ToString())),
                Difficulty.HARD.ToString(), Category.Dragon.ToString(),
            new List<SkillEnemy> { new SkillEnemy(Guid.NewGuid(), Guid.NewGuid().ToString("N"), 4000, 60,
                        new IncreasingState<int>(60, StrategyIncreasing.ADDITIVE.ToString()))});
        }

        public static IEnumerable<Hero> GetHeroes()
        {
            yield return new Hero(new Guid("00000000-0000-0000-0000-000000000001"), Guid.NewGuid().ToString("N"),
                new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())),
                new State<decimal>(1000, new IncreasingState<decimal>(100, StrategyIncreasing.ADDITIVE.ToString())),
                new List<SkillHero> { new SkillHero(Guid.NewGuid(), Guid.NewGuid().ToString("N"), 500, 
                                    new IncreasingState<int>(30, StrategyIncreasing.PERCENTAGE.ToString())) });
            yield return new Hero(new Guid("00000000-0000-0000-0000-000000000002"), Guid.NewGuid().ToString("N"),
                new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())),
                new State<int>(100, new IncreasingState<int>(10, StrategyIncreasing.ADDITIVE.ToString())),
                new State<decimal>(1000, new IncreasingState<decimal>(100, StrategyIncreasing.ADDITIVE.ToString())),
                new List<SkillHero> { new SkillHero(Guid.NewGuid(), Guid.NewGuid().ToString("N"), 500,
                                    new IncreasingState<int>(30, StrategyIncreasing.PERCENTAGE.ToString())) });
        }

        public static IEnumerable<User> GetUsers()
        {
            yield return new User(new Guid("00000000-0000-0000-0000-000000000001"), $"email-admin-predefined@email.com", "password", "admin", DateTime.UtcNow);
            yield return new User(new Guid("00000000-0000-0000-0000-000000000002"), $"email-user-predefined@email.com", "password", "user", DateTime.UtcNow);
        }

        public static IEnumerable<Player> GetPlayers()
        {
            var heroes = GetHeroes().ToList();
            var players = new List<Player>();
            players.Add(new Player(new Guid("00000000-0000-0000-0000-000000000001"), Guid.NewGuid().ToString("N"), heroes[0].AsAssign(), 1, 0, heroes[0].BaseRequiredExperience.Value, new Guid("00000000-0000-0000-0000-000000000001")));
            players.Add(new Player(new Guid("00000000-0000-0000-0000-000000000002"), Guid.NewGuid().ToString("N"), heroes[1].AsAssign(), 1, 0, heroes[1].BaseRequiredExperience.Value, new Guid("00000000-0000-0000-0000-000000000002")));
            return players;
        }

        public static IEnumerable<Map> GetMaps()
        {
            var enemies = GetEnemies().ToList();
            yield return new Map(new Guid("00000000-0000-0000-0000-000000000001"), Guid.NewGuid().ToString("N"), Difficulty.EASY.ToString(),
                new List<Enemies> { new Enemies(enemies[0].AsAssign(), 2), new Enemies(enemies[2].AsAssign(), 1) });
            yield return new Map(new Guid("00000000-0000-0000-0000-000000000002"), Guid.NewGuid().ToString("N"), Difficulty.MEDIUM.ToString(),
                new List<Enemies> { new Enemies(enemies[1].AsAssign(), 3), new Enemies(enemies[3].AsAssign(), 2) });
            yield return new Map(new Guid("00000000-0000-0000-0000-000000000003"), Guid.NewGuid().ToString("N"), Difficulty.HARD.ToString(),
                new List<Enemies> { new Enemies(enemies[0].AsAssign(), 3), new Enemies(enemies[1].AsAssign(), 2), new Enemies(enemies[2].AsAssign(), 2),
                            new Enemies(enemies[3].AsAssign(), 1), new Enemies(enemies[4].AsAssign(), 1) });
            yield return new Map(new Guid("00000000-0000-0000-0000-000000000004"), Guid.NewGuid().ToString("N"), Difficulty.EASY.ToString(),
                new List<Enemies> { new Enemies(enemies[0].AsAssign(), 3) });
            yield return new Map(new Guid("00000000-0000-0000-0000-000000000005"), Guid.NewGuid().ToString("N"), Difficulty.EASY.ToString(),
                new List<Enemies> { new Enemies(enemies[0].AsAssign(), 3) });
        }

        public static IEnumerable<Battle> GetBattles()
        {
            var maps = GetMaps().ToList();
            var players = GetPlayers().ToList();
            var battle1 = new Battle(new Guid("00000000-0000-0000-0000-000000000001"), DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000002"), BattleInfo.Starting.ToString(), maps[4]);
            battle1.AddBattleStateAtPrepare(new BattleState(new Guid("00000000-0000-0000-0000-000000000001"), BattleStatus.Prepare.ToString(), battle1.Id, players[0], DateTime.UtcNow));
            var battle2 = new Battle(new Guid("00000000-0000-0000-0000-000000000002"), DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000002"), BattleInfo.Starting.ToString(), maps[4]);
            battle2.AddBattleStateAtPrepare(new BattleState(new Guid("00000000-0000-0000-0000-000000000002"), BattleStatus.Prepare.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            battle2.AddBattleStateAtInAction(new BattleState(new Guid("00000000-0000-0000-0000-000000000003"), BattleStatus.InAction.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            var battle3 = new Battle(new Guid("00000000-0000-0000-0000-000000000003"), DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000002"), BattleInfo.Starting.ToString(), maps[4]);
            battle3.AddBattleStateAtPrepare(new BattleState(new Guid("00000000-0000-0000-0000-000000000004"), BattleStatus.Prepare.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            battle3.AddBattleStateAtInAction(new BattleState(new Guid("00000000-0000-0000-0000-000000000005"), BattleStatus.InAction.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            var enemies = GetEnemies().ToList();
            battle3.AddKilledEnemy(enemies[0].Id);
            battle3.AddKilledEnemy(enemies[0].Id);
            battle3.AddKilledEnemy(enemies[0].Id);
            var battle4 = new Battle(new Guid("00000000-0000-0000-0000-000000000004"), DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000002"), BattleInfo.Starting.ToString(), maps[4]);
            battle4.AddBattleStateAtPrepare(new BattleState(new Guid("00000000-0000-0000-0000-000000000006"), BattleStatus.Prepare.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            battle4.AddBattleStateAtInAction(new BattleState(new Guid("00000000-0000-0000-0000-000000000007"), BattleStatus.InAction.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            var battle5 = new Battle(new Guid("00000000-0000-0000-0000-000000000005"), DateTime.UtcNow, new Guid("00000000-0000-0000-0000-000000000002"), BattleInfo.Starting.ToString(), maps[4]);
            battle5.AddBattleStateAtPrepare(new BattleState(new Guid("00000000-0000-0000-0000-000000000008"), BattleStatus.Prepare.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            battle5.AddBattleStateAtInAction(new BattleState(new Guid("00000000-0000-0000-0000-000000000009"), BattleStatus.InAction.ToString(), battle2.Id, players[0], DateTime.UtcNow));
            yield return battle1; 
            yield return battle2;
            yield return battle3;
            yield return battle4;
            yield return battle5;
        }
    }
}
