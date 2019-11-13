using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.Application.Dto;
using Basket.Application.UseCases;
using Basket.ApplicationUnitTests.Infrastructure;
using Basket.Domain.Model;
using Basket.Infrastructure.InfrastructureExceptions;
using FluentAssertions;
using Xunit;

namespace Basket.ApplicationUnitTests.UseCases
{
    public class GetCustomerBasketTests
    {
        [Fact]
        public async Task GetCustomerBasketWhenNoExists()
        {
            var id = "Test";
            var custmerId = CustomerId.Crate(id);
            var repo = new FakeCustomerBasketRepository(custmerId, false, false);
            var useCase = new GetCustomerBasket(repo);
            var subject = await useCase.Execute(new GetCustomerBasketRequest(custmerId));

            subject.IsSucceed.Should().BeTrue();
            subject.Match(ok =>
            {
                ok.Id.Should().NotBeEmpty();
                ok.CustomerId.Should().Be(id);
                ok.Items.Should().BeEmpty();
            }, err => { throw err; });
        }
        
        
        [Fact]
        public async Task GetCustomerBasketWhenExists()
        {
            var id = "Test";
            var custmerId = CustomerId.Crate(id);
            var fakeItem = Item.Create("Test", 10);
            var fakeCustomerBasket =
                CustomerBasket.Create(BasketId.Create(), custmerId, new List<Item>() {fakeItem});
            var repo = new FakeCustomerBasketRepository(custmerId, true, false, fakeCustomerBasket);
            var useCase = new GetCustomerBasket(repo);
            var subject = await useCase.Execute(new GetCustomerBasketRequest(custmerId));

            subject.IsSucceed.Should().BeTrue();
            subject.Match(ok =>
            {
                ok.Id.Should().NotBeEmpty();
                ok.CustomerId.Should().Be(id);
                ok.Items.Should().Contain(x => x.Id == fakeItem.Id.Value);
            }, err => { throw err; });
        }
        
        [Fact]
        public async Task GetCustomerBasketWhenException()
        {
            var id = "Test";
            var custmerId = CustomerId.Crate(id);
            var fakeItem = Item.Create("Test", 10);
            var fakeCustomerBasket =
                CustomerBasket.Create(BasketId.Create(), custmerId, new List<Item>() {fakeItem});
            var repo = new FakeCustomerBasketRepository(custmerId, true, true, fakeCustomerBasket);
            var useCase = new GetCustomerBasket(repo);
            var subject = await useCase.Execute(new GetCustomerBasketRequest(custmerId));

            subject.IsSucceed.Should().BeFalse();
            subject.Match(ok =>
            {
                Assert.False(true);
            }, err =>
            {
                err.Should().BeOfType<BasketRedisException>();
                (err as BasketRedisException).CustomerId.Should().Be(custmerId);
            });
        }
    }
}