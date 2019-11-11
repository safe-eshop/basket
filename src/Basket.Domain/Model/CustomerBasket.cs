using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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


        public RopResult<Unit> AddItems(List<Item> items)
        {
            foreach (var item in items)
            {
                var oldItem = _items.FirstOrDefault(x => x.Id.Equals(item.Id));
                if (oldItem is null)
                {
                    _items.Add(item);
                }
                else
                {
                    var newItem = oldItem.IncreaseQuantity(item.Quantity);
                    int index = _items.IndexOf(oldItem);
                    _items[index] = newItem;
                }
            }
            return RopResult.UnitResult;
        }
    }
}