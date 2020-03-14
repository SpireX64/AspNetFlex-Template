using System;
using AspNetFlex.Domain.Interactions.Users.Models;
using NodaTime;

namespace Estimate.Domain.UnitTests.Utils.Factories
{
    public class FakeUserFactory
    {
        public Guid UserId { get; } = Guid.NewGuid();
        public string Name { get; set; } = "fakeuser";
        public string Email { get; set; } = "fake@example.com";
        public string FirstName { get; set; } = "Fake";
        public string LastName { get; set; } = "Faker";

        public IClock Clock { get; set; } = SystemClock.Instance;
        
        public UserModel Get(Guid? userId = null)
        {
            return new UserModel
            {
                Id = userId ?? UserId,
                Name = Name,
                Email = Email,
                RegistrationDate = Clock.GetCurrentInstant()
            };
        }
    }
}