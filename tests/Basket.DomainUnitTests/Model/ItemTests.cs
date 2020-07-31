using System;
using Basket.Domain.Types;
using FluentAssertions;
using Xunit;

namespace Basket.DomainUnitTests.Model
{
    public class ItemTests
    {
        [Theory]
        [InlineData(0, 1, 1)]
        [InlineData(1, 2, 3)]
        [InlineData(2, 3, 5)]
        [InlineData(3, 4, 7)]
        public void TestIncreaseQuantity(int start, int increaseVal, int expected)
        {
            var item = Item.Create(Guid.NewGuid(), start);
            var subject = item.IncreaseQuantity(increaseVal);
            subject.Quantity.Should().Be(expected);
        }
        
        [Theory]
        [InlineData(0, 1, 0)]
        [InlineData(3, 2, 1)]
        [InlineData(4, 3, 1)]
        [InlineData(55, 4, 51)]
        public void TestDecreaseQuantity(int start, int increaseVal, int expected)
        {
            var item = Item.Create(Guid.NewGuid(), start);
            var subject = item.DecreaseQuantity(increaseVal);
            subject.Quantity.Should().Be(expected);
        }
    }
}