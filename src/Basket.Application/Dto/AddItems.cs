using System;
using System.Collections.Generic;

namespace Basket.Application.Dto
{
    public class AddItemsRequest
    {
        public AddItemsRequest(Guid customerId, IEnumerable<(Guid productId, int quantity)> items)
        {
            CustomerId = customerId;
            Items = items;
        }

        public Guid CustomerId { get; }
        public IEnumerable<(Guid productId, int quantity)> Items { get; }      
    }
}