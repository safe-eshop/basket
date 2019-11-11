using System;
using System.Collections.Generic;
using System.Collections.Immutable;
namespace Basket.Domain.Model
{
    public sealed class CustomerBasket
    {
        
        private readonly List<Item> _items;
        public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
        public BasketId Id { get; }
        public CustomerId CustomerId { get; }

        private CustomerBasket(BasketId id, CustomerId customerId)
        {
            _items = new List<Item>();
            Id = id;
            CustomerId = customerId;
        }
        
        public static CustomerBasket Empty(BasketId id, CustomerId customerId)
        {
            return new CustomerBasket(id, customerId);
        }
    }
}