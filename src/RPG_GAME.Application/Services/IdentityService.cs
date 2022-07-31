﻿using Microsoft.AspNetCore.Identity;
using RPG_GAME.Application.Auth;
using RPG_GAME.Application.DTO;
using RPG_GAME.Application.Exceptions.Auth;
using RPG_GAME.Application.Time;
using RPG_GAME.Core.Entities;
using RPG_GAME.Core.Repositories;
using RPG_GAME.Core.ValueObjects;

namespace RPG_GAME.Application.Services
{
    internal sealed class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthManager _authManager;
        private readonly IClock _clock;

        public IdentityService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,
            IAuthManager authManager, IClock clock)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authManager = authManager;
            _clock = clock;
        }

        public async Task<JsonWebToken> SignInAsync(SignInDto dto)
        {
            var user = await _userRepository.GetAsync(dto.Email.ToLowerInvariant());
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }

            VerifyPassword(user.Password, dto.Password);

            if (!user.IsActive)
            {
                throw new UserNotActiveException(user.Id);
            }

            var jwt = GenerateToken(user);

            return jwt;
        }

        public async Task SignUpAsync(SignUpDto dto)
        {
            dto.Id = Guid.NewGuid();
            var email = dto.Email.ToLowerInvariant();
            Password.From(dto.Password);
            var user = await _userRepository.GetAsync(email);

            if (user is not null)
            {
                throw new EmailInUseException();
            }

            var password = _passwordHasher.HashPassword(default, dto.Password);
            user = new User
            {
                Id = dto.Id,
                Email = Email.From(email),
                Password = password,
                Role = dto.Role?.ToLowerInvariant() ?? "user",
                CreatedAt = _clock.CurrentDate(),
                IsActive = true
            };
            await _userRepository.AddAsync(user);
        }

        private void VerifyPassword(string correctPassword, string password)
        {
            if (_passwordHasher.VerifyHashedPassword(default, correctPassword, password) ==
                PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException();
            }
        }

        private JsonWebToken GenerateToken(User user)
        {
            var jwt = _authManager.CreateToken(user.Id.ToString(), user.Role);
            jwt.Email = user.Email.Value;
            return jwt;
        }
    }
}
