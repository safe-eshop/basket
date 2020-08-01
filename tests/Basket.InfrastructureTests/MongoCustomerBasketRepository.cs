using System;
using System.Threading.Tasks;
using Basket.Infrastructure.Repository;
using Basket.Persistence;
using FluentAssertions;
using MongoDB.Driver;
using Xunit;
using static LanguageExt.FSharp;
namespace Basket.InfrastructureTests
{
    public class MongoCustomerBasketRepositoryTests
    {
        private IMongoClient _client;
        private IMongoDatabase _database;

        public MongoCustomerBasketRepositoryTests()
        {
            MappingExtensions.AddCollections();
            
            _client = new MongoClient("mongodb://root:rootpassword@127.0.0.1:27017");
            _database = _client.GetDatabase($"{nameof(Basket)}{nameof(InfrastructureTests)}");
        }

        [Fact]
        public async Task GetNotExistingCustomerBasket()
        {
            var repo = new MongoCustomerBasketRepository(_client, _database);
            var result = await repo.Get(Guid.NewGuid());
            fs(result).IsNone.Should().BeTrue();
        }
    }
}