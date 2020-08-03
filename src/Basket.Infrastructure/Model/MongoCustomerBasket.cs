using System;
using System.Collections.Generic;
using System.Linq;
using Basket.Domain.Types;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Basket.Infrastructure.Model
{
    public class MongoCustomerBasketItem
    {
        [BsonId] 
        [BsonRepresentation(BsonType.String)]
        public Guid ItemId { get; set; }
        [BsonElement] public int Quantity { get; set; }
    }

    public class MongoCustomerBasket
    {
        [BsonId] 
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        
        [BsonElement] 
        [BsonRepresentation(BsonType.String)]
        public Guid CustomerId { get; set; }
        [BsonElement] public IList<MongoCustomerBasketItem> Items { get; set; }


        public static CustomerBasket MapToCustomerBasket(MongoCustomerBasket basket)
        {
            var items = basket.Items.Select(x => Item.Create(x.ItemId, x.Quantity)).ToList();
            return new CustomerBasket(basket.Id, basket.CustomerId, items);
        } 
        
        public static MongoCustomerBasket MapToMongoCustomerBasket(CustomerBasket basket)
        {
            var items = basket.Items
                .Select(x => new MongoCustomerBasketItem() {ItemId = x.Id, Quantity = x.Quantity}).ToList();
            return new MongoCustomerBasket()
            {
                Id = basket.Id,
                CustomerId = basket.CustomerId,
                Items = items
            };
        } 
    }
}