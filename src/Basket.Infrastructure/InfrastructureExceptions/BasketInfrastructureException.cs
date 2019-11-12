using System;
using Basket.Domain.Exceptions;
using Basket.Domain.Model;

namespace Basket.Infrastructure.InfrastructureExceptions
{
    public abstract class BasketInfrastructureException : BasketDomainException
    {
        protected BasketInfrastructureException(CustomerId customerId) : base(customerId)
        {
        }

        protected BasketInfrastructureException(string message, CustomerId customerId) : base(message, customerId)
        {
        }

        protected BasketInfrastructureException(string message, Exception innerException, CustomerId customerId) : base(message, innerException, customerId)
        {
        }
    }
}