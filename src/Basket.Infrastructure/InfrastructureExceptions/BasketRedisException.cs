using System;
using Basket.Domain.Model;

namespace Basket.Infrastructure.InfrastructureExceptions
{
    public class BasketRedisException : BasketInfrastructureException
    {
        public BasketRedisException(CustomerId customerId) : base(customerId)
        {
        }

        public BasketRedisException(string message, CustomerId customerId) : base(message, customerId)
        {
        }

        public BasketRedisException(string message, Exception innerException, CustomerId customerId) : base(message, innerException, customerId)
        {
        }
    }
}