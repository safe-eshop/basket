using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Basket.Common.Types;

namespace Basket.Domain.Model
{
    public sealed class CustomerBasket
    {
        private readonly List<Item> _items;
        public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
        public BasketId Id { get; }
        public CustomerId CustomerId { get; }

        private CustomerBasket(BasketId id, CustomerId customerId) : this(id, customerId, new List<Item>())
        {
        }

        private CustomerBasket(BasketId id, CustomerId customerId, List<Item> items)
        {
            _items = items;
            Id = id;
            CustomerId = customerId;
        }

        public static CustomerBasket Empty(BasketId id, CustomerId customerId)
        {
            return new CustomerBasket(id, customerId);
        }

        public static CustomerBasket Empty(CustomerId customerId)
        {
            return new CustomerBasket(BasketId.Create(), customerId);
        }

        public static CustomerBasket Create(BasketId id, CustomerId customerId, List<Item> items)
        {
            return new CustomerBasket(id, customerId, items);
        }


        public RopResult<CustomerBasket> AddItems(List<Item> items)
        {
            throw new NotImplementedException();
        }
    }
}