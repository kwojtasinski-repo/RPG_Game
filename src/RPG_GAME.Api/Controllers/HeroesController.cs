using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    [Authorize]
    public class HeroesController : BaseController
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpGet]
        public async Task<IEnumerable<HeroDto>> GetAll()
        {
            return await _heroService.GetAllAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<HeroDetailsDto>> Get(Guid id)
        {
            return OkOrNotFound(await _heroService.GetAsync(id));
        }

        [Authorize(Policy = "is-admin")]
        [HttpPost]
        public async Task<ActionResult> Add(HeroDto heroDto)
        {
            await _heroService.AddAsync(heroDto);
            return CreatedAtAction(nameof(Get), new { Id = heroDto.Id }, default);
        }

        [Authorize(Policy = "is-admin")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Update(Guid id, HeroDto heroDto)
        {
            heroDto.Id = id;
            await _heroService.UpdateAsync(heroDto);
            return NoContent();
        }

        [Authorize(Policy = "is-admin")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _heroService.RemoveAsync(id);
            return NoContent();
        }
    }
}
