﻿using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Maps;
using RPG_GAME.Core.Entities.Players;
using System;

namespace RPG_GAME.UnitTests.Fixtures
{
    internal static class BattleFixture
    {
        public static Battle CreateDefaultBattle(DateTime startDate, Guid userId, Map map, string battleInfo = null)
        {
            return new Battle(Guid.NewGuid(), startDate, userId, battleInfo ?? "Starting", map);
        }

        public static Battle CreateBattleAtPrepare(DateTime startDate, Guid userId, Map map, Player player)
        {
            var battle = CreateDefaultBattle(startDate, userId, map);
            battle.AddBattleStateAtPrepare(BattleState.Prepare(battle.Id, player, startDate));
            return battle;
        }

        public static Battle CreateBattleInProgress(DateTime startDate, Guid userId, Map map, Player player)
        {
            var battle = CreateBattleAtPrepare(startDate, userId, map, player);
            battle.AddBattleStateAtInProgress(BattleState.InAction(battle.Id, player, startDate));
            return battle;
        }
    }
}