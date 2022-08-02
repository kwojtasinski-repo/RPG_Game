using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO.Heroes;
using RPG_GAME.Application.Services;

namespace RPG_GAME.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HeroDetailsDto>> Get(Guid id)
        {
            return await _heroService.GetAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Add(HeroDto heroDto)
        {
            await _heroService.AddAsync(heroDto);
            return CreatedAtAction(nameof(Get), new { Id = heroDto.Id }, default);
        }

        [HttpPut]
        public async Task<ActionResult> Update(HeroDto heroDto)
        {
            await _heroService.UpdateAsync(heroDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _heroService.RemoveAsync(id);
            return NoContent();
        }
    }
}
