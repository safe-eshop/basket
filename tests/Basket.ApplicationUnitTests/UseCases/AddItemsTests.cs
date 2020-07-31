using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.Application.Dto;
using Basket.Application.UseCases;
using Basket.ApplicationUnitTests.Infrastructure;
using Basket.Domain.Model;
using FluentAssertions;
using Xunit;

namespace Basket.ApplicationUnitTests.UseCases
{
    public class AddItemsTests
    {
        [Fact]
        public async Task AddItemWhenCustomerBasketNotExistsInDatabase()
        {
            var id = "Test";
            var custmerId = CustomerId.Crate(id);
            var repo = new FakeCustomerBasketRepository(custmerId, false, false);
            var useCase = new AddItems(repo);
            var subject = await useCase.Execute(new AddItemRequest(custmerId,
                new List<(ItemId productId, ItemQuantity quantity)>() {(new ItemId("1"), new ItemQuantity(1))}));

            subject.IsSucceed.Should().BeTrue();
        }
    }
}