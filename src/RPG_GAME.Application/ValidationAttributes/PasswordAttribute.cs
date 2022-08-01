﻿using RPG_GAME.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace RPG_GAME.Application.ValidationAttributes
{
    internal sealed class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(
            object? value, ValidationContext validationContext)
        {
            var password = (string)value!;
            Password.From(password);
            return ValidationResult.Success;
        }
    }
}