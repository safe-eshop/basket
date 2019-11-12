using System;
using System.Runtime.Serialization;
using Basket.Domain.Model;

namespace Basket.Application.ApplicationsErrors
{
    public class NoItemsToAddApplicationException : BasketApplicationException
    {
        public NoItemsToAddApplicationException(CustomerId customerId) : base(customerId)
        {
        }

        public NoItemsToAddApplicationException(string message, CustomerId customerId) : base(message, customerId)
        {
        }

        public NoItemsToAddApplicationException(string message, Exception innerException, CustomerId customerId) : base(message, innerException, customerId)
        {
        }
    }
}