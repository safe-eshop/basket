using System.Collections.Generic;

namespace Basket.Application.Dto
{
    public class CustomerBasketDto
    {
        public CustomerBasketDto(string id, string customerId, List<ItemDto> items)
        {
            CustomerId = customerId;
            Items = items;
            Id = id;
        }

        public string CustomerId { get; }
        public List<ItemDto> Items { get; }
        public string Id { get; }
    }

    public class ItemDto
    {
        public string Id { get; }
        public int Quantity { get; }

        public ItemDto(string id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}