using FluentAssertions;
using RPG_GAME.Core.Entities.Users;
using RPG_GAME.Core.Exceptions.Common;
using RPG_GAME.Core.Exceptions.Players;
using System;
using Xunit;

namespace RPG_GAME.UnitTests.Entities.Users
{
    public class UserTests
    {
        private User Act(string email, string password, string role, DateTime createdAt, bool isActive)
            => new User(Guid.NewGuid(), email, password, role, createdAt, isActive);

        [Fact]
        public void should_create()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";

            var user = User.Create(email, password, date, role);

            user.Should().NotBeNull();
            user.Email.Should().NotBeNull();
            user.Email.ToString().Should().Be(email);
            user.Password.Should().NotBeNull();
            user.CreatedAt.Should().Be(date);
            user.Role.Should().Be(role);
        }

        [Fact]
        public void given_invalid_email_should_throw_an_exception()
        {
            var email = "email";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";
            var expectedException = new InvalidEmailException(email);

            var exception = Record.Exception(() => Act(email, password, role, date, true));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_email_when_change_value_should_throw_an_exception()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";
            var user = User.Create(email, password, date, role);
            var emailModified = "ema";
            var expectedException = new InvalidEmailException(emailModified);

            var exception = Record.Exception(() => user.ChangeEmail(emailModified));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_email()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";
            var user = User.Create(email, password, date, role);
            var emailModified = "em@gmail.com";

            user.ChangeEmail(emailModified);

            user.Email.Value.Should().Be(emailModified);
        }

        [Fact]
        public void given_invalid_password_should_throw_an_exception()
        {
            var email = "email@email.com";
            var password = "";
            var date = DateTime.UtcNow;
            var role = "user";
            var expectedException = new InvalidPasswordException();

            var exception = Record.Exception(() => Act(email, password, role, date, true));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void given_invalid_password_when_change_value_should_throw_an_exception()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";
            var user = User.Create(email, password, date, role);
            var expectedException = new InvalidPasswordException();

            var exception = Record.Exception(() => user.ChangePassword(""));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_password()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";
            var user = User.Create(email, password, date, role);
            var passwordModified = "P3A4ssW0RD";

            user.ChangePassword(passwordModified);

            user.Password.Should().Be(passwordModified);
        }

        [Fact]
        public void given_invalid_role_should_throw_an_exception()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "";
            var expectedException = new InvalidRoleException();

            var exception = Record.Exception(() => Act(email, password, role, date, true));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        public void given_invalid_role_when_change_value_should_throw_an_exception()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";
            var user = User.Create(email, password, date, role);
            var expectedException = new InvalidRoleException();

            var exception = Record.Exception(() => user.ChangeRole(""));

            exception.Should().NotBeNull();
            exception.Should().BeOfType(expectedException.GetType());
            exception.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public void should_change_role()
        {
            var email = "email@email.com";
            var password = "PAssword!2";
            var date = DateTime.UtcNow;
            var role = "user";
            var user = User.Create(email, password, date, role);
            var newRole = "admin";

            user.ChangeRole(newRole);

            user.Role.Should().Be(newRole);
        }
    }
}
