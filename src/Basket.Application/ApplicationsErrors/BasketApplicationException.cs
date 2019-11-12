using System;
using System.Runtime.Serialization;
using Basket.Domain;
using Basket.Domain.Exceptions;
using Basket.Domain.Model;

namespace Basket.Application.ApplicationsErrors
{
    public abstract class BasketApplicationException : BasketDomainException
    {
        protected BasketApplicationException(CustomerId customerId) : base(customerId)
        {
        }

        protected BasketApplicationException(string message, CustomerId customerId) : base(message, customerId)
        {
        }

        protected BasketApplicationException(string message, Exception innerException, CustomerId customerId) : base(message, innerException, customerId)
        {
        }
    }
}