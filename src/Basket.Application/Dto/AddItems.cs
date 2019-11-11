using System.Collections.Generic;

namespace Basket.Application.Dto
{
    public class AddItemsRequest
    {
        public AddItemsRequest(string customerId, IEnumerable<(string productId, int quantity)> items)
        {
            CustomerId = customerId;
            Items = items;
        }

        public string CustomerId { get; }
        public IEnumerable<(string productId, int quantity)> Items { get; }      
    }
}