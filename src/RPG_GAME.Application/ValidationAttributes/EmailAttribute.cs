using RPG_GAME.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace RPG_GAME.Application.ValidationAttributes
{
    internal sealed class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var email = (string)value!;
            Email.From(email);
            return ValidationResult.Success;
        }
    }
}
