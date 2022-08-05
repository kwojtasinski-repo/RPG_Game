using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO.Maps;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    public class MapsController : BaseController
    {
        private readonly IMapService _mapService;

        public MapsController(IMapService mapService)
        {
            _mapService = mapService;
        }

        [HttpGet]
        public async Task<IEnumerable<MapDto>> GetAll()
        {
            return await _mapService.GetAllAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MapDto>> Get(Guid id)
        {
            return OkOrNotFound(await _mapService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(MapDto mapDto)
        {
            await _mapService.AddAsync(mapDto);
            return CreatedAtAction(nameof(Get), new { Id = mapDto.Id }, default);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, MapDto mapDto)
        {
            mapDto.Id = id;
            await _mapService.UpdateAsync(mapDto);
            return NoContent();
        }
    }
}
