using System;
using System.Runtime.Serialization;
using Basket.Domain.Types;

namespace Basket.Application.ApplicationsErrors
{
    public abstract class BasketApplicationException : BasketException
    {
        protected BasketApplicationException()
        {
        }

        protected BasketApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        protected BasketApplicationException(Guid id) : base(id)
        {
        }
    }
}