using System;
using System.Threading.Tasks;
using Basket.Domain.Types;
using Basket.Infrastructure.Repository;
using Basket.Persistence;
using FluentAssertions;
using LanguageExt.UnsafeValueAccess;
using MongoDB.Driver;
using Xunit;
using static LanguageExt.FSharp;

namespace Basket.InfrastructureTests
{
    public class MongoDbFixture : IDisposable
    {
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public MongoDbFixture()
        {
            MappingExtensions.AddCollections();

            Client = new MongoClient("mongodb://root:rootpassword@127.0.0.1:27017");
            Database = Client.GetDatabase($"{nameof(Basket)}{nameof(InfrastructureTests)}");
        }


        public void Dispose()
        {
            Database.DropCollection(MongoCustomerBasketRepository.CustomerBasketCollection);
        }
    }

    public class MongoCustomerBasketRepositoryTests : IClassFixture<MongoDbFixture>
    {
        private MongoDbFixture _mongoDbFixture;

        public MongoCustomerBasketRepositoryTests(MongoDbFixture mongoDbFixture)
        {
            _mongoDbFixture = mongoDbFixture;
        }

        [Fact]
        public async Task GetNotExistingCustomerBasket()
        {
            var repo = new MongoCustomerBasketRepository(_mongoDbFixture.Client, _mongoDbFixture.Database);
            var result = await repo.GetByCustomerId(Guid.NewGuid());
            fs(result).IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task GetExistingCustomerBasket()
        {
            var id = Guid.NewGuid();
            var item = Item.Create(Guid.NewGuid(), 10);
            var repo = new MongoCustomerBasketRepository(_mongoDbFixture.Client, _mongoDbFixture.Database);
            
            var res = await repo.AddItem(id, item);
            res.IsOk.Should().BeTrue();
            
            var result = await repo.GetByCustomerId(id);
            var opt = fs(result);
            opt.IsSome.Should().BeTrue();
            var subject = opt.ValueUnsafe();
            subject.CustomerId.Should().Be(id);
            subject.Items.Should().Contain(item);
            
        }
        
        [Fact]
        public async Task UpdateExistingCustomerBasket()
        {
            var id = Guid.NewGuid();
            var item = Item.Create(Guid.NewGuid(), 10);
            var repo = new MongoCustomerBasketRepository(_mongoDbFixture.Client, _mongoDbFixture.Database);
            
            // Insert New
            var res = await repo.AddItem(id, item);
            res.IsOk.Should().BeTrue();
            
            
            // Get new and check if basket and item exists
            var result = await repo.GetByCustomerId(id);
            var subject = fs(result);
            
            
            subject.IsSome.Should().BeTrue();
            subject.ValueUnsafe().CustomerId.Should().Be(id);
            subject.ValueUnsafe().Items.Should().Contain(item);
            
            
            // Try update basket=
            res = await repo.AddItem(id, item);
            res.IsOk.Should().BeTrue();
            
            result = await repo.GetByCustomerId(id);
            subject = fs(result);
            subject.IsSome.Should().BeTrue();
            subject.ValueUnsafe().CustomerId.Should().Be(id);
            // Quantity Should be updated
            subject.ValueUnsafe().Items.Should().Contain(item.IncreaseQuantity(item.Quantity));
        }
        
        
        [Fact]
        public async Task GetExistingCustomerBasketAndRemoveIt()
        {
            var id = Guid.NewGuid();
            var item = Item.Create(Guid.NewGuid(), 10);
            var repo = new MongoCustomerBasketRepository(_mongoDbFixture.Client, _mongoDbFixture.Database);
            
            // Insert New
            var res = await repo.AddItem(id, item);
            res.IsOk.Should().BeTrue();
            
            
            // Get new and check if basket and item exists
            var result = await repo.GetByCustomerId(id);
            var subject = fs(result);
            
            
            subject.IsSome.Should().BeTrue();
            subject.ValueUnsafe().CustomerId.Should().Be(id);
            subject.ValueUnsafe().Items.Should().Contain(item);
            
            
            // Try update basket=
            res = await repo.Checkout(id);
            res.IsOk.Should().BeTrue();
            
            result = await repo.GetByCustomerId(id);
            subject = fs(result);
            subject.IsNone.Should().BeTrue();
        }
    }
}