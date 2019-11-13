using System.Collections.Generic;
using Basket.Domain.Model;

namespace Basket.Application.Dto
{
    public class AddItemsRequest
    {
        public AddItemsRequest(CustomerId customerId, IEnumerable<(ItemId productId, ItemQuantity quantity)> items)
        {
            CustomerId = customerId;
            Items = items;
        }

        public CustomerId CustomerId { get; }
        public IEnumerable<(ItemId productId, ItemQuantity quantity)> Items { get; }      
    }
}