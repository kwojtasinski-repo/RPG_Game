using Microsoft.AspNetCore.Mvc;
using RPG_GAME.Application.DTO;
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
        public async Task<ActionResult> Add(HeroDetailsDto heroDetailsDto)
        {
            await _heroService.AddAsync(heroDetailsDto);
            return CreatedAtAction(nameof(Get), new { Id = heroDetailsDto.Id }, default);
        }

        [HttpPut]
        public async Task<ActionResult> Update(HeroDetailsDto heroDetailsDto)
        {
            await _heroService.UpdateAsync(heroDetailsDto);
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
