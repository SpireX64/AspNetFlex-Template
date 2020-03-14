using System;
using System.Collections.Generic;
using System.Threading;
using AspNetFlex.Domain.Infrastructure.Utils;
using AspNetFlex.Domain.Interactions.Users.Commands.AuthenticateUser;
using AspNetFlex.Domain.Interactions.Users.Exceptions;
using AspNetFlex.Domain.Interactions.Users.Models;
using AspNetFlex.Domain.Interactions.Users.Repositories;
using AspNetFlex.Domain.Interactions.Users.Utils;
using AspNetFlex.Domain.UnitTests.Utils.Factories;
using Estimate.Domain.UnitTests.Utils.Factories;
using Microsoft.Extensions.Configuration;
using Moq;
using NodaTime;
using NodaTime.Testing;
using NUnit.Framework;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Domain.UnitTests.Interactions.Users
{
    [TestFixture]
    public class AuthenticateUserCommandTest
    {
        private const string SigningKey = "h2lbFRwnK3KnSjM19Z4LrcdaKontQGRZo1Foo1nZ";

        private readonly Mock<IUsersRepository> _authRepositoryMock = new Mock<IUsersRepository>();
        private Mock<IRepositoryProvider> _repositoryProviderMock;
        private IConfiguration _configuration;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            _repositoryProviderMock = FakeRepositoryProviderFactory.CreateRepositoryProviderMock(
                () => _authRepositoryMock.Object);
            
            _configuration = ConfigurationFactory.FromDictionary(new Dictionary<string, string>
            {
                [$"{AuthUtils.Jwt.ConfigKeys.Section}:{AuthUtils.Jwt.ConfigKeys.SigningKey}"] = SigningKey,
                [$"{AuthUtils.Jwt.ConfigKeys.Section}:{AuthUtils.Jwt.ConfigKeys.LifetimeKey}"] = "10m"
            });
        }

        [TearDown]
        public void AfterEach()
        {
            _authRepositoryMock.Reset();
        }
        
        [Test]
        public void AuthenticateCommand_AuthUser_SuccessAuth()
        {
            // ---- Arrange ----
            
            const string email = "test@email.com";
            const string name = "Test user";
            const string password = "ADSqwe123";
            var passwordHash = AuthUtils.GetMd5Hash(password);

            var clock = new FakeClock(SystemClock.Instance.GetCurrentInstant());
            var testUser = new UserIdentityModel(
                Guid.NewGuid(), 
                email,
                name,
                "user", 
                passwordHash,
                clock.GetCurrentInstant());
            
            _authRepositoryMock.Setup(r => r.GetUserIdentity(email))
                .ReturnsAsync(() => testUser);

            var command = new AuthenticateUserCommand(email, password);
            var handler = new AuthenticateUserCommandHandler(_repositoryProviderMock.Object, _configuration)
                {
                    Clock = clock
                };

            AuthAccessModel result = null;
            
            var lifetime = DurationUtils.FromString(
                _configuration[$"{AuthUtils.Jwt.ConfigKeys.Section}:{AuthUtils.Jwt.ConfigKeys.LifetimeKey}"]);
            
            // ---- Act ----
            
            Assert.DoesNotThrowAsync(async () => {
                result = await handler.Handle(command, CancellationToken.None);
            });
            
            clock.Advance(lifetime); // for check expires token instant
            
            // ---- Assert ----
            
            Assert.IsNotNull(result);
            Assert.AreEqual(testUser.Id, result.Id);
            Assert.AreEqual(testUser.Email, result.Email);
            Assert.AreEqual(clock.GetCurrentInstant(), result.ExpiresAt);
            Assert.AreEqual(clock.GetCurrentInstant(), result.ExpiresAt);
            Assert.AreEqual("user", testUser.Role);
            Assert.IsNotEmpty(result.Token);
            
            _authRepositoryMock.Verify(r => r.GetUserIdentity(email), Times.Once);
        }

        [Test]
        public void AuthenticateCommand_PresentInvalidEmail_ThrowsException()
        {
            // ---- Arrange ----
            
            const string invalidEmail = "invalid_email";
            const string password = "ADSqwe123";
            var command = new AuthenticateUserCommand(invalidEmail, password);
            var handler = new AuthenticateUserCommandHandler(_repositoryProviderMock.Object, _configuration);
            
            // ---- Act & Assert ----
            
            var exception = Assert.ThrowsAsync<InvalidEmailFormatException>(
                async () => await handler.Handle(command, CancellationToken.None));

            Assert.AreEqual(invalidEmail, exception.Email);
        }

        [Test]
        public void AuthenticateCommand_PresentNewEmail_ThrowsException()
        {
            // ---- Arrange ----
            
            const string email = "test@email.com";
            const string password = "ADSqwe123";
            var command = new AuthenticateUserCommand(email, password);
            var handler = new AuthenticateUserCommandHandler(_repositoryProviderMock.Object, _configuration);
            
            // ---- Act & Assert ----
            
            var exception = Assert.ThrowsAsync<UserNotFoundByEmailException>(
                async () => await handler.Handle(command, CancellationToken.None));
            
            _authRepositoryMock.Verify(r => r.GetUserIdentity(email), Times.Once);
            StringAssert.AreEqualIgnoringCase(email, exception.Email);
        }

        [Test]
        public void AuthenticateCommand_PresentWrongPassword_ThrowsException()
        {
            // ---- Arrange ----
            
            const string email = "test@email.com";
            const string name = "Test user";
            const string wrongPassword = "wrong-password";
            const string password = "ADSqwe123";
            var passwordHash = AuthUtils.GetMd5Hash(password);

            var testUser = new UserIdentityModel(
                Guid.NewGuid(), 
                email, 
                "Test user",
                name, 
                passwordHash,
                Instant.MinValue);
            
            _authRepositoryMock.Setup(r => r.GetUserIdentity(email))
                .ReturnsAsync(() => testUser);

            var command = new AuthenticateUserCommand(email, wrongPassword);
            var handler = new AuthenticateUserCommandHandler(_repositoryProviderMock.Object, _configuration);
            
            // ---- Act & Assert ----
            
            Assert.ThrowsAsync<WrongPasswordException>(
                async () => await handler.Handle(command, CancellationToken.None));
            
            _authRepositoryMock.Verify(r => r.GetUserIdentity(email), Times.Once);
        }
    }
}