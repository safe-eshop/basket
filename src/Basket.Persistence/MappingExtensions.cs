using System;
using Basket.Infrastructure.Model;
using MongoDB.Bson.Serialization;

namespace Basket.Persistence
{
    public static class MappingExtensions
    {
        public static void AddCollections()
        {
            BsonClassMap.RegisterClassMap<MongoCustomerBasketItem>();
            BsonClassMap.RegisterClassMap<MongoCustomerBasket>();
        }
    }
}