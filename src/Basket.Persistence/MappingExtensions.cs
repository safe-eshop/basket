using System;
using Basket.Infrastructure.Model;
using Basket.Infrastructure.Repository;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Basket.Persistence
{
    public static class MappingExtensions
    {
        public static void AddCollections(this IMongoDatabase db)
        {
            BsonClassMap.RegisterClassMap<MongoCustomerBasketItem>();
            BsonClassMap.RegisterClassMap<MongoCustomerBasket>();
            db.CreateCollection(MongoCustomerBasketRepository.CustomerBasketCollection);
        }
    }
}