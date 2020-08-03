using System;
using System.Collections.Generic;
using Basket.Domain.Types;
using FluentAssertions;
using Xunit;

namespace Basket.DomainUnitTests.Model
{
    public class CustomerBasketTests
    {
        [Fact]
        public void AddItemsWhenBasketIsEmpty()
        {
            var id = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var basket = CustomerBasket.Empty(id);
            var subject = basket.AddItem(Item.Create(productId, 10));
            subject.CustomerId.Should().Be(id);
            subject.Items.Should().Contain(Item.Create(productId, 10));
        }
        
        [Fact]
        public void AddItemsWhenBasketIsNotEmpty()
        {
            var id = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var basket = CustomerBasket.Empty(id);
            var subject = basket.AddItem(Item.Create(productId, 10));
            subject.CustomerId.Should().Be(id);
            subject.Items.Should().Contain(Item.Create(productId, 10));
            var productId2 = Guid.NewGuid();
            subject = subject.AddItem(Item.Create(productId, 10));
            subject = subject.AddItem(Item.Create(productId2, 10));
            subject.CustomerId.Should().Be(id);
            subject.Items.Should().Contain(Item.Create(productId, 20));
            subject.Items.Should().Contain(Item.Create(productId2, 10));
        }
        
        [Fact]
        public void RemoveItemsWhenBasketIsEmpty()
        {
            var id = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var basket = CustomerBasket.Empty(id);
            var subject = basket.RemoveItem(Item.Create(productId, 10));
            subject.CustomerId.Should().Be(id);
            subject.Items.Should().BeEmpty();
        }
        
        [Fact]
        public void RemoveItemsWhenBasketIsNotEmpty()
        {
            var id = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var basket = CustomerBasket.Empty(id);
            var subject = basket.AddItem(Item.Create(productId, 10));
            subject.CustomerId.Should().Be(id);
            subject.Items.Should().Contain(Item.Create(productId, 10));
            var productId2 = Guid.NewGuid();
            subject = subject.RemoveItem(Item.Create(productId, 5));
            subject = subject.RemoveItem(Item.Create(productId2, 10));
            subject.CustomerId.Should().Be(id);
            subject.Items.Should().Contain(Item.Create(productId, 5));
            subject.Items.Should().NotContain(Item.Create(productId2, 10));
        }
    }
}