using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Exceptions.Heroes;
using RPG_GAME.Application.Exceptions.Players;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Entities.Players;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IHeroRepository _heroRepository;

        public PlayerService(IPlayerRepository playerRepository, IHeroRepository heroRepository)
        {
            _playerRepository = playerRepository;
            _heroRepository = heroRepository;
        }

        public async Task AddAsync(AddPlayerDto playerDto)
        {
            var hero = await _heroRepository.GetAsync(playerDto.HeroId);

            if (hero is null)
            {
                throw new HeroNotFoundException(playerDto.HeroId);
            }

            var heroAssing = hero.AsAssign();
            var player = Player.Create(playerDto.Name, heroAssing, hero.BaseRequiredExperience.Value, playerDto.UserId);
            await _playerRepository.AddAsync(player);
        }

        public async Task RemoveAsync(Guid id)
        {
            var playerExists = await _playerRepository.GetAsync(id);

            if (playerExists is null)
            {
                throw new PlayerNotFoundException(id);
            }

            await _playerRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(PlayerDto playerDto)
        {
            var player = await _playerRepository.GetAsync(playerDto.Id);

            if (player is null)
            {
                throw new PlayerNotFoundException(playerDto.Id);
            }

            player.ChangeName(playerDto.Name);
            player.ChangeCurrentExp(playerDto.CurrentExp);
            player.ChangeRequiredExp(playerDto.RequiredExp);

            await _playerRepository.UpdateAsync(player);
        }

        public async Task<PlayerDto> GetAsync(Guid id)
        {
            var player = await _playerRepository.GetAsync(id);
            return player.AsDto();
        }

        public async Task<IEnumerable<PlayerDto>> GetAllAsync()
        {
            var players = await _playerRepository.GetAllAsync();
            return players.Select(p => p.AsDto());
        }
    }
}
