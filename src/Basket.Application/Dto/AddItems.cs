using System;
using System.Collections.Generic;
using Basket.Domain.Types;

namespace Basket.Application.Dto
{
    public class AddItemRequest
    {
        public AddItemRequest(Guid customerId, Item item)
        {
            CustomerId = customerId;
            Item = item;
        }

        public Guid CustomerId { get; }
        public Item Item { get; }
    }
}