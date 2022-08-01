using RPG_GAME.Application.DTO;
using RPG_GAME.Core.Repositories;

namespace RPG_GAME.Application.Services
{
    internal sealed class MapService : IMapService
    {
        private readonly IMapRepository _mapRepository;

        public MapService(IMapRepository mapRepository)
        {
            _mapRepository = mapRepository;
        }

        public Task<MapDto> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MapDto>> GetAllAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
