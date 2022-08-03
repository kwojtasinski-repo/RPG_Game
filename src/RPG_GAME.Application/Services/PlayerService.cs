using RPG_GAME.Application.DTO.Players;
using RPG_GAME.Application.Mappings;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class PlayerService : IPlayerService
    {
        private  readonly  IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task AddAsync(PlayerDto playerDto)
        {
            await _playerRepository.AddAsync(playerDto.AsEntity());
        }

        public async Task RemoveAsync(Guid id)
        {
            await _playerRepository.DeleteAsync(id);
        }

        public async Task UpdateAsync(PlayerDto playerDto)
        {
            await _playerRepository.UpdateAsync(playerDto.AsEntity());
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
