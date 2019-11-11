using System;
using System.Runtime.Serialization;
using Basket.Domain.Model;

namespace Basket.Application.ApplicationsErrors
{
    public class BasketException : Exception
    {
        public CustomerId CustomerId { get; }
        
        public BasketException(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        protected BasketException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public BasketException(string message, CustomerId customerId) : base(message)
        {
            CustomerId = customerId;
        }

        public BasketException(string message, Exception innerException, CustomerId customerId) : base(message, innerException)
        {
            CustomerId = customerId;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(CustomerId)}: {CustomerId}";
        }
    }
}