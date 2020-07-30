using System;
using System.Runtime.Serialization;

namespace Basket.Application.ApplicationsErrors
{
    public class NoItemsToAddApplicationException : BasketApplicationException
    {
        public NoItemsToAddApplicationException()
        {
        }

        public NoItemsToAddApplicationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NoItemsToAddApplicationException(Guid id) : base(id)
        {
        }
    }
}