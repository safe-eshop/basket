using System;
using Basket.Infrastructure.Model;
using MongoDB.Bson.Serialization;

namespace Basket.Persistence
{
    public class MappingExtensions
    {
        public void AddCollections()
        {
            BsonClassMap.RegisterClassMap<MongoCustomerBasketItem>();
            BsonClassMap.RegisterClassMap<MongoCustomerBasket>();
        }
    }
}