using FluentAssertions;
using RPG_GAME.Application.Managers;
using RPG_GAME.Core.Common;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Maps;
using System;
using System.Collections.Generic;
using Xunit;

namespace RPG_GAME.UnitTests.Managers
{
    public class EnemyAttackManagerTests
    {
        [Fact]
        public void should_attack_with_base_damage()
        {
            var enemy = new EnemyAssign(Guid.NewGuid(), "Enemy", 100, 100, 10, 1000, Difficulty.EASY.ToString(), Category.Knight.ToString());

            var attackDto = _enemyAttackManager.AttackHeroWithDamage(enemy);

            attackDto.Should().NotBeNull();
            attackDto.AttackName.Should().Be(nameof(EnemyAssign.Attack));
            attackDto.Damage.Should().Be(enemy.Attack);
        }

        [Fact]
        public void should_attack_with_skill()
        {
            var skillEnemyAssign = new SkillEnemyAssign(Guid.NewGuid(), "skill", 1250, 100);
            var enemy = new EnemyAssign(Guid.NewGuid(), "Enemy", 100, 100, 10, 1000, Difficulty.EASY.ToString(), Category.Knight.ToString(), new List<SkillEnemyAssign> { skillEnemyAssign });

            var attackDto = _enemyAttackManager.AttackHeroWithDamage(enemy);

            attackDto.Should().NotBeNull();
            attackDto.AttackName.Should().Be(skillEnemyAssign.Name);
            attackDto.Damage.Should().Be(skillEnemyAssign.Attack);
        }

        private readonly IEnemyAttackManager _enemyAttackManager;

        public EnemyAttackManagerTests()
        {
            _enemyAttackManager = new EnemyAttackManager();
        }
    }
}
