using RPG_GAME.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace RPG_GAME.Application.ValidationAttributes
{
    internal sealed class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            try
            {
                var password = (string)value!;
                Password.From(password);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}
