using System;

namespace Basket.Domain.Model
{
    public sealed class BasketId
    {
        public Guid Value { get; }

        public BasketId(Guid value)
        {
            Value = value;
        }

        public static BasketId Create()
        {
            return new BasketId(Guid.NewGuid());
        }
    }
}