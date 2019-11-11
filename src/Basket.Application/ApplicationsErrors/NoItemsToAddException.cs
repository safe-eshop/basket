using System;
using System.Runtime.Serialization;
using Basket.Domain.Model;

namespace Basket.Application.ApplicationsErrors
{
    public class NoItemsToAddException : BasketException
    {
        public NoItemsToAddException(CustomerId customerId) : base(customerId)
        {
        }

        public NoItemsToAddException(string message, CustomerId customerId) : base(message, customerId)
        {
        }

        public NoItemsToAddException(string message, Exception innerException, CustomerId customerId) : base(message, innerException, customerId)
        {
        }
    }
}