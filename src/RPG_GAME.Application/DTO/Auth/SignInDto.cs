using RPG_GAME.Application.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RPG_GAME.Application.DTO.Auth
{
    public class SignInDto
    {
        [Email]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
