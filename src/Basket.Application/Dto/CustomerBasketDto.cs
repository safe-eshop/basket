using System;
using System.Collections.Generic;

namespace Basket.Application.Dto
{
    public class CustomerBasketDto
    {
        public CustomerBasketDto(Guid id, Guid customerId, List<ItemDto> items)
        {
            CustomerId = customerId;
            Items = items;
            Id = id;
        }

        public Guid CustomerId { get; }
        public List<ItemDto> Items { get; }
        public Guid Id { get; }
    }

    public class ItemDto
    {
        public Guid Id { get; }
        public int Quantity { get; }

        public ItemDto(Guid id, int quantity)
        {
            Id = id;
            Quantity = quantity;
        }
    }
}