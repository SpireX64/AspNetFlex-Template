using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetFlex.Domain.Interactions.Users.Exceptions;
using AspNetFlex.Domain.Interactions.Users.Queries.GetUserById;
using AspNetFlex.Domain.Interactions.Users.Repositories;
using AspNetFlex.Domain.UnitTests.Utils.Factories;
using Estimate.Domain.UnitTests.Utils.Factories;
using Moq;
using NUnit.Framework;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Domain.UnitTests.Interactions.Users
{
    [TestFixture]
    public class GetUserByIdQueryTest
    {
        private Mock<IRepositoryProvider> _repositoryProviderMock;
        private readonly FakeUserFactory _userFactory = new FakeUserFactory();
        private readonly Mock<IUsersRepository> _usersRepositoryMock = new Mock<IUsersRepository>();
        
        [OneTimeSetUp]
        public void BeforeAll()
        {
            _repositoryProviderMock = FakeRepositoryProviderFactory.CreateRepositoryProviderMock(
                () => _usersRepositoryMock.Object);
        }

        [TearDown]
        public void AfterEach()
        {
            _usersRepositoryMock.Reset();
        }
        
        [Test]
        public async Task GetUserByIdQuery_PresentExistsUserId_ReturnsUser()
        {
            // ---- Arrange ----
            var userId = Guid.NewGuid();
            _usersRepositoryMock.Setup(r => r.Get(userId))
                .ReturnsAsync((Guid id) => _userFactory.Get(id));
            
            var query = new GetUserByIdQuery(userId);
            var handler = new GetUserByIdQueryHandler(_repositoryProviderMock.Object);
            
            // ---- Act ----
            var user = await handler.Handle(query, CancellationToken.None);

            // ---- Assert ----
            _usersRepositoryMock.Verify(r => r.Get(userId), Times.Once);
            Assert.NotNull(user);
            Assert.AreEqual(userId, user.Id);
        }

        [Test]
        public async Task GetUserByIdQuery_PresentNotExistsUserId_ReturnsNull()
        {
            // ---- Arrange ----
            var query = new GetUserByIdQuery(Guid.Empty);
            var handler = new GetUserByIdQueryHandler(_repositoryProviderMock.Object);

            // ---- Act ----
            var user = await handler.Handle(query, CancellationToken.None);
            
            // ---- Assert ----
            Assert.IsNull(user);
        }
        
        [Test]
        public void GetUserByIdQuery_PresentNotExistsUserIdAllowException_ThrowException()
        {
            // ---- Arrange ----
            var query = new GetUserByIdQuery(Guid.Empty) {ThrowIfNotExists = true};
            var handler = new GetUserByIdQueryHandler(_repositoryProviderMock.Object);
            
            // ---- Act & Assert
            var exception = Assert.ThrowsAsync<UserNotFoundByIdException>(async () =>
                await handler.Handle(query, CancellationToken.None));
            Assert.AreEqual(query.UserId, exception.UserId);
        }
    }
}