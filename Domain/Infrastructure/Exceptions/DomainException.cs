using System;

namespace AspNetFlex.Domain.Infrastructure.Exceptions
{
    public abstract class DomainException: Exception
    {
        public DomainException()
        {
            // invoke base constructor
            // Exception();
        }

        public DomainException(string reason) : base(reason)
        {
            // invoke base ctor
            // Exception(reason);
        }

        public DomainException(string reason, Exception exception): base(reason, exception)
        {
            // invoke base ctor
            // Exception(reason, exception);
        }
    }
}