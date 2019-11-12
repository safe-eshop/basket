using System;
using Basket.Domain.Model;

namespace Basket.Domain.Exceptions
{
    public abstract class BasketDomainException : Exception
    {
        protected CustomerId CustomerId { get; }

        protected BasketDomainException(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        protected BasketDomainException(string message, CustomerId customerId) : base(message)
        {
            CustomerId = customerId;
        }

        protected BasketDomainException(string message, Exception innerException, CustomerId customerId) : base(message, innerException)
        {
            CustomerId = customerId;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(CustomerId)}: {CustomerId}";
        }
    }
}