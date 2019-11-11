using System.Collections.Generic;
using Basket.Domain.Model;
using FluentAssertions;
using Xunit;

namespace Basket.DomainUnitTests.Model
{
    public class CustomerBasketTests
    {
        [Fact]
        public void AddItemsWhenBasketIsEmpty()
        {
            var basket = CustomerBasket.Empty(CustomerId.Crate("Test"));
            var subject = basket.AddItems(new List<Item>() {Item.Create("test", 10)});
            subject.IsSucceed.Should().BeTrue();
            basket.Id.Should().NotBeNull();
            basket.Items.Should().Contain(Item.Create("test", 10));
            basket.CustomerId.Should().Be(CustomerId.Crate("Test"));
        }

        [Fact]
        public void AddItemsWhenAnyNotExistsInCustomerBasket()
        {
            var basket = CustomerBasket.Create(BasketId.Create(), CustomerId.Crate("Test"), new List<Item>() {Item.Create("test1", 10)});
            var subject = basket.AddItems(new List<Item>() {Item.Create("test2", 10)});
            subject.IsSucceed.Should().BeTrue();
            basket.Id.Should().NotBeNull();
            basket.Items.Should().Contain(Item.Create("test1", 10));
            basket.Items.Should().Contain(Item.Create("test2", 10));
            basket.CustomerId.Should().Be(CustomerId.Crate("Test"));
        }
        
        [Fact]
        public void AddItemsWhenExistsInCustomerBasket()
        {
            var basket = CustomerBasket.Create(BasketId.Create(), CustomerId.Crate("Test"), new List<Item>() {Item.Create("test", 10)});
            var subject = basket.AddItems(new List<Item>() {Item.Create("test", 10)});
            subject.IsSucceed.Should().BeTrue();
            basket.Id.Should().NotBeNull();
            basket.Items.Should().Contain(Item.Create("test", 20));
            basket.CustomerId.Should().Be(CustomerId.Crate("Test"));
        }
    }
}