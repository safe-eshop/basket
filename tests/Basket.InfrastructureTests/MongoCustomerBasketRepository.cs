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
            using var repo = new MongoCustomerBasketRepository(_mongoDbFixture.Client, _mongoDbFixture.Database);
            var result = await repo.Get(Guid.NewGuid());
            fs(result).IsNone.Should().BeTrue();
        }

        [Fact]
        public async Task GetExistingCustomerBasket()
        {
            var id = Guid.NewGuid();
            var item = Item.Create(Guid.NewGuid(), 10);
            using var repo = new MongoCustomerBasketRepository(_mongoDbFixture.Client, _mongoDbFixture.Database);
            
            var res = await repo.InsertOrUpdate(CustomerBasket.Empty(id).AddItem(item));
            res.IsOk.Should().BeTrue();
            
            var result = await repo.Get(id);
            var subject = fs(result);
            subject.IsSome.Should().BeTrue();
            subject.ValueUnsafe().CustomerId.Should().Be(id);
            subject.ValueUnsafe().Items.Should().Contain(item);
        }
    }
}