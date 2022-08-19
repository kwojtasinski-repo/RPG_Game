using RPG_GAME.Application.Exceptions.Battles;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities.Battles;
using RPG_GAME.Core.Entities.Players;

namespace RPG_GAME.Application.Managers
{
    internal sealed class BattleManager : IBattleManager
    {
        private readonly IClock _clock;

        public BattleManager(IClock clock)
        {
            _clock = clock;
        }

        public Task<Player> CompleteBattle(Battle battle, Player player)
        {
            var enemiesToKill = battle.Map.Enemies.Count();
            
            foreach(var enemy in battle.Map.Enemies)
            {
                enemiesToKill += enemy.Quantity;
            }
            
            var battleState = BattleState.Completed(battle.Id, player, _clock.CurrentDate());

            if (battle.EnemiesKilled.Count() != enemiesToKill)
            {
                var playerNotChanged = battle.EndBattle(_clock.CurrentDate(), "Lost", battleState);
                return Task.FromResult(playerNotChanged);
            }

            var playerToUpdate = battle.EndBattle(_clock.CurrentDate(), "Won", battleState);
            return Task.FromResult(playerToUpdate);
        }

        public Task<BattleEvent> CreateBattleEvent(Guid battleId, Player player, string attackInfo)
        {
            var damage = GetAttack(attackInfo, player);
            // healthEnemy = damage - currentHelpEnemy;
            var healthHero = player.Hero.Health;
            return Task.FromResult(new BattleEvent(Guid.NewGuid(), battleId, 
                new Core.Entities.Battles.Actions.FightAction(player.Hero.Id, 
                        player.Hero.Character.ToString(), player.Hero.HeroName, 
                        damage, healthHero, attackInfo), _clock.CurrentDate()));
        }


        private int GetAttack(string attack, Player player)
        {
            if (attack == null)
            {
                throw new InvalidAttackException(attack);
            }

            if (attack.ToLowerInvariant() == "attack")
            {
                return player.Hero.Attack;
            }

            var skills = player.Hero.Skills;

            foreach(var skill in skills)
            {
                if (skill.Name == attack)
                {
                    return skill.Attack;
                }
            }

            throw new InvalidAttackException(attack);
        }
    }
}
