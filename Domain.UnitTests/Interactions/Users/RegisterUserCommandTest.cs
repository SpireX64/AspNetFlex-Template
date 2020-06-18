using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetFlex.Domain.Interactions.Users.Commands.RegisterUser;
using AspNetFlex.Domain.Interactions.Users.Exceptions;
using AspNetFlex.Domain.Interactions.Users.Models;
using AspNetFlex.Domain.Interactions.Users.Repositories;
using AspNetFlex.Domain.UnitTests.Utils.Factories;
using Estimate.Domain.UnitTests.Utils.Factories;
using Moq;
using NodaTime;
using NodaTime.Testing;
using NUnit.Framework;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Domain.UnitTests.Interactions.Users
{
    [TestFixture]
    public class RegisterUserCommandTest
    {
        
        private readonly Mock<IUsersRepository> _authRepositoryMock = new Mock<IUsersRepository>();
        private Mock<IRepositoryProvider> _repositoryProviderMock;
        private FakeClock _fakeClock;

        [OneTimeSetUp]
        public void BeforeAll()
        {
            _repositoryProviderMock = FakeRepositoryProviderFactory.CreateRepositoryProviderMock(
                () => _authRepositoryMock.Object);
        }

        [SetUp]
        public void BeforeEach()
        {
            _fakeClock = new FakeClock(SystemClock.Instance.GetCurrentInstant());
        }
        
        [TearDown]
        public void AfterEach()
        {
            _authRepositoryMock.Reset();
        }

        [Test]
        public async Task RegistrationCommand_RegistrationNewUser_SuccessRegistration()
        {
            // ---- Arrange ----
            
            const string name = "test user";
            const string email = "test@email.com";
            const string password = "ADSqwe123";

            _authRepositoryMock.Setup(r => r.RegisterUser(It.IsAny<UserIdentityModel>()))
                .ReturnsAsync((UserIdentityModel user) => new UserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    RegistrationDate = user.RegistrationDate
                });

            var command = new RegisterUserCommand(email, name, password);
            var handler = new RegisterUserCommandHandler(_repositoryProviderMock.Object)
            {
                Clock = _fakeClock
            };

            // ---- Act ----

            var result = await handler.Handle(command, CancellationToken.None);
            
            // ---- Assert ----
            
            Assert.NotNull(result);
            Assert.AreNotEqual(Guid.Empty, result.Id);
            Assert.AreEqual(email, result.Email);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(_fakeClock.GetCurrentInstant(), result.RegistrationDate);
            
            _authRepositoryMock.Verify(r => r.IsUserIdentityExists(email), Times.Once);
            _authRepositoryMock.Verify(r => r.RegisterUser(It.IsAny<UserIdentityModel>()), Times.Once);
        }

        [Test]
        public void RegistrationCommand_PresentInvalidFormattedEmail_ThrowsException()
        {
            // ---- Arrange ----
            
            const string invalidEmail = "invalid_email";
            const string name = "test user";
            const string password = "ADSqwe123";
            
            var command = new RegisterUserCommand(invalidEmail, name, password);
            var handler = new RegisterUserCommandHandler(_repositoryProviderMock.Object);

            // ---- Act & Assert

            var exception = Assert.ThrowsAsync<InvalidEmailFormatException>(async () => 
                await handler.Handle(command, CancellationToken.None));
            
            Assert.AreEqual(invalidEmail, exception.Email);
        }

        [Test]
        public void RegistrationCommand_PresentAlreadyExistsEmail_ThrowsException()
        {
            // ---- Arrange ----
            const string name = "test user";
            const string email = "test@email.com";
            const string password = "ADSqwe123";

            _authRepositoryMock.Setup(r => r.IsUserIdentityExists(email))
                .ReturnsAsync(true);
            
            var command = new RegisterUserCommand(email, name, password);
            var handler = new RegisterUserCommandHandler(_repositoryProviderMock.Object);
            
            // ---- Act & Assert ----
            
            var exception = Assert.ThrowsAsync<UserAlreadyExistsException>(async () => 
                await handler.Handle(command, CancellationToken.None));
            
            _authRepositoryMock.Verify(r => r.IsUserIdentityExists(email), Times.Once);
            Assert.AreEqual(email, exception.Email);
        }

        [Test]
        public void RegistrationCommand_PresentInvalidFormattedName_ThrowsException()
        {
            // ---- Arrange ----
            
            const string badName = "";
            const string email = "test@email.com";
            const string password = "ADSqwe123";
            
            var command = new RegisterUserCommand(email, badName, password);
            var handler = new RegisterUserCommandHandler(_repositoryProviderMock.Object);
            
            // ---- Act & Assert ----
            
            var exception = Assert.ThrowsAsync<InvalidNameFormatException>(async () => 
                await handler.Handle(command, CancellationToken.None));
            
            Assert.AreEqual(badName, exception.Name);
        }

        [Test]
        public void RegistrationCommand_PresentWeakPassword_ThrowsException()
        {
            // ---- Arrange ----
            
            const string name = "test user";
            const string email = "test@email.com";
            const string weakPassword = "qwerty12";
            
            var command = new RegisterUserCommand(email, name, weakPassword);
            var handler = new RegisterUserCommandHandler(_repositoryProviderMock.Object);
            
            // ---- Act & Assert ----
            
            Assert.ThrowsAsync<WeakPasswordException>(async () => 
                await handler.Handle(command, CancellationToken.None));
        }
    }
}