using FluentAssertions;
using RPG_GAME.Core.Entities.Battles.Actions;
using RPG_GAME.Core.Entities.Common;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Exceptions.Battles;
using System;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Battles
{
    public class FightActionTests
    {
        [Fact]
        public void should_create_fight_action()
        {
            var characterId = Guid.NewGuid();
            var characterType = CharacterType.PLAYER;
            var name = "Player";
            var damage = 20;
            var health = 10;
            var attackInfo = HeroAssign.Action.BASE_ATTACK;

            var fightAction = new FightAction(characterId, characterType.ToString(), name, damage, health, attackInfo);

            fightAction.Should().NotBeNull();
            fightAction.CharacterId.Should().Be(characterId);
            fightAction.Character.Should().Be(characterType);
            fightAction.Name.Should().Be(name);
            fightAction.DamageDealt.Should().Be(damage);
            fightAction.Health.Should().Be(health);
            fightAction.AttackInfo.Should().Be(attackInfo);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abcd")]
        [InlineData("1234")]
        public void given_invalid_character_type_should_throw_an_exception(string characterType)
        {
            var characterId = Guid.NewGuid();
            var name = "Player";
            var damage = 20;
            var health = 10;
            var attackInfo = HeroAssign.Action.BASE_ATTACK;
            var expectedException = new InvalidCharacterTypeException(characterType);

            var exception = Record.Exception(() => new FightAction(characterId, characterType, name, damage, health, attackInfo));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
            ((InvalidCharacterTypeException)exception).Character.Should().Be(expectedException.Character);
        }
    }
}
