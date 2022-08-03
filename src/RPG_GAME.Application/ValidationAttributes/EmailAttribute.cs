using RPG_GAME.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace RPG_GAME.Application.ValidationAttributes
{
    internal sealed class EmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            try
            {
                var email = (string)value!;
                Email.From(email);
                return ValidationResult.Success;
            }
            catch (Exception ex)
            {
                return new ValidationResult(ex.Message);
            }
        }
    }
}
