using RPG_GAME.Application.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RPG_GAME.Application.DTO
{
    public class SignUpDto
    {
        public Guid Id { get; set; }

        [Email]
        [Required]
        public string Email { get; set; }

        [Required]
        [Password]
        public string Password { get; set; }

        public string? Role { get; set; }
    }
}
