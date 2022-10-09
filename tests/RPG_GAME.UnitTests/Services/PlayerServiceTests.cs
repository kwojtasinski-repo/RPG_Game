using FluentAssertions;
using Moq;
using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Events;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Exceptions.Players;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Application.Messaging;
using RPG_GAME.Application.Services;
using RPG_GAME.Core.Entities.Heroes;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Repositories;
using RPG_GAME.UnitTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RPG_GAME.UnitTests.Services
{
    public class PlayerServiceTests
    {
        [Fact]
        public async Task should_add()
        {
            var user = AddDefaultUser();
            var hero = AddDefaultHero();
            var dto = new AddPlayerDto { Name = "Player#1", HeroId = hero.Id, UserId = user.Id };

            await _playerService.AddAsync(dto);

            dto.Id.Should().NotBe(Guid.Empty);
            _playerRepository.Verify(p => p.AddAsync(It.IsAny<Player>()), times: Times.Once());
            _messageBroker.Verify(p => p.PublishAsync(It.IsAny<IEvent>()), times: Times.Once());
        }

        [Fact]
        public async Task given_not_existing_hero_when_add_should_throw_an_exception()
        {
            var dto = new AddPlayerDto { Name = "Player#1", HeroId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            var expectedException = new HeroNotFoundException(dto.HeroId);

            var exception = await Record.ExceptionAsync(() => _playerService.AddAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existing_user_when_add_should_throw_an_exception()
        {
            var hero = AddDefaultHero();
            var dto = new AddPlayerDto { Name = "Player#1", HeroId = hero.Id, UserId = Guid.NewGuid() };
            var expectedException = new UserNotFoundException(dto.UserId);

            var exception = await Record.ExceptionAsync(() => _playerService.AddAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_update()
        {
            var player = AddDefaultPlayer();
            var dto = new UpdatePlayerDto
            {
                Id = player.Id,
                CurrentExp = 100,
                HeroAttack = 200,
                HeroHealth = 300,
                HeroHealLvl = 100,
                Name = "Player#23",
                RequiredExp = 1200
            };

            await _playerService.UpdateAsync(dto);

            _playerRepository.Verify(p => p.UpdateAsync(It.IsAny<Player>()), times: Times.Once());
        }

        [Fact]
        public async Task given_not_existing_player_when_update_should_throw_an_exception()
        {
            var dto = new UpdatePlayerDto { Id = Guid.NewGuid(), CurrentExp = 100, HeroAttack = 200, HeroHealth = 300, HeroHealLvl = 100, Name = "Player#23", RequiredExp = 1200 };
            var expectedException = new PlayerNotFoundException(dto.Id);

            var exception = await Record.ExceptionAsync(() => _playerService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task given_not_existing_hero_skill_when_update_should_throw_an_exception()
        {
            var player = AddDefaultPlayer();
            var skillId = Guid.NewGuid();
            var dto = new UpdatePlayerDto
            {
                Id = player.Id,
                CurrentExp = 100,
                HeroAttack = 200,
                HeroHealth = 300,
                HeroHealLvl = 100,
                Name = "Player#23",
                RequiredExp = 1200,
                HeroSkills = new List<UpdateHeroSkillDto> { new UpdateHeroSkillDto { SkillId = skillId, Attack = 1000 } }
            };
            var expectedException = new HeroSkillNotFoundException(skillId);

            var exception = await Record.ExceptionAsync(() => _playerService.UpdateAsync(dto));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task should_delete()
        {
            var player = AddDefaultPlayer();

            await _playerService.RemoveAsync(player.Id);

            _playerRepository.Verify(p => p.DeleteAsync(It.IsAny<Guid>()), times: Times.Once());
            _messageBroker.Verify(p => p.PublishAsync(It.IsAny<IEvent>()), times: Times.Once());
        }

        [Fact]
        public async Task given_not_existing_player_when_delete_should_throw_an_exception()
        {
            var id = Guid.NewGuid();
            var expectedException = new PlayerNotFoundException(id);

            var exception = await Record.ExceptionAsync(() => _playerService.RemoveAsync(id));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        public Player AddDefaultPlayer()
        {
            var hero = AddDefaultHero();
            var user = AddDefaultUser();
            var player = Player.Create("Player#1", hero.AsAssign(), 1000, user.Id);
            _playerRepository.Setup(p => p.GetAsync(player.Id)).ReturnsAsync(player);
            return player;
        }

        public Hero AddDefaultHero()
        {
            var hero = EntitiesFixture.CreateDefaultHero();
            _heroRepository.Setup(h => h.GetAsync(hero.Id)).ReturnsAsync(hero);
            return hero;
        }

        public User AddDefaultUser()
        {
            var user = User.Create("email@email.com", "PasW0RDd12", DateTime.UtcNow, "user");
            _userRepository.Setup(u => u.GetAsync(user.Id)).ReturnsAsync(user);
            _userRepository.Setup(u => u.ExistsAsync(user.Id)).ReturnsAsync(true);
            return user;
        }

        private readonly IPlayerService _playerService;
        private readonly Mock<IPlayerRepository> _playerRepository;
        private readonly Mock<IHeroRepository> _heroRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMessageBroker> _messageBroker; 

        public PlayerServiceTests()
        {
            _playerRepository = new Mock<IPlayerRepository>();
            _heroRepository = new Mock<IHeroRepository>();
            _userRepository = new Mock<IUserRepository>();
            _messageBroker = new Mock<IMessageBroker>();
            _playerService = new PlayerService(_playerRepository.Object, _heroRepository.Object, _userRepository.Object, _messageBroker.Object);
        }
    }
}
