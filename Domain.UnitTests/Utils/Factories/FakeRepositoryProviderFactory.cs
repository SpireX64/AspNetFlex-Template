using System;
using Moq;
using SpireX.RepositoryProvider.Abstractions;

namespace AspNetFlex.Domain.UnitTests.Utils.Factories
{
    public static class FakeRepositoryProviderFactory
    {
        public static Mock<IRepositoryProvider> CreateRepositoryProviderMock<T>(Func<T> builder) where T : IRepository
        {
            var providerMock = new Mock<IRepositoryProvider>();
            providerMock.Setup(e => e.GetRepository<T>())
                .Returns(builder);
            return providerMock;
        }
    }
}