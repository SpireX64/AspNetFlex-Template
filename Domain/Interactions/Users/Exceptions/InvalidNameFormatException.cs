using AspNetFlex.Domain.Infrastructure.Exceptions;

namespace AspNetFlex.Domain.Interactions.Users.Exceptions
{
    public class InvalidNameFormatException : DomainException
    {
        public string Name { get; }

        public InvalidNameFormatException(string name)
        {
            Name = name;
        }
    }
}