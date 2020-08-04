using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Basket.Domain;
using Basket.Domain.Repository;
using Basket.Domain.Types;
using Basket.Infrastructure.Model;
using LanguageExt;
using Microsoft.FSharp.Core;
using MongoDB.Driver;
using Unit = Microsoft.FSharp.Core.Unit;
using static LanguageExt.Prelude;
using FSharpx;
using LanguageExt.UnsafeValueAccess;
using static LanguageExt.Prelude;

namespace Basket.Infrastructure.Repository
{
    public class MongoCustomerBasketRepository : ICustomerBasketRepository
    {
        public const string CustomerBasketCollection = nameof(CustomerBasket);
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<MongoCustomerBasket> _mongoCustomerBasket;

        public MongoCustomerBasketRepository(IMongoClient client, IMongoDatabase database)
        {
            _client = client;
            _database = database;
            _mongoCustomerBasket = _database.GetCollection<MongoCustomerBasket>(CustomerBasketCollection);
        }

        public async Task<FSharpOption<CustomerBasket>> GetByCustomerId(Guid customerId)
        {
            var result = await _mongoCustomerBasket.Find(GetByCustomerIdFilter(customerId)).FirstOrDefaultAsync();
            return Optional(result).Map(MongoCustomerBasket.MapToCustomerBasket).ToFSharp();
        }

        public async Task<FSharpResult<CustomerBasket, Exception>> AddItem(Guid customerId, Item item)
        {
            using var session = await _client.StartSessionAsync();
            session.StartTransaction();
            var basket = await _mongoCustomerBasket.Find(session, GetByCustomerIdFilter(customerId))
                .FirstOrDefaultAsync();
            var basketOpt = Optional(basket).Map(MongoCustomerBasket.MapToCustomerBasket)
                .IfNone(() => CustomerBasket.Empty(customerId));
            var newBasket = basketOpt.AddItem(item);
            await _mongoCustomerBasket.ReplaceOneAsync(session, x => x.Id == newBasket.Id,
                MongoCustomerBasket.MapToMongoCustomerBasket(newBasket), new ReplaceOptions {IsUpsert = true});

            await session.CommitTransactionAsync();
            return FSharpResult<CustomerBasket, Exception>.NewOk(newBasket);
        }

        public async Task<FSharpResult<CustomerBasket, Exception>> RemoveItem(Guid customerId, Item item)
        {
            using var session = await _client.StartSessionAsync();
            var basket = await _mongoCustomerBasket.Find(session, GetByCustomerIdFilter(customerId))
                .FirstOrDefaultAsync();
            var basketOpt = Optional(basket)
                .Map(MongoCustomerBasket.MapToCustomerBasket).Map(x => x.RemoveItem(item))
                .Where(x => !x.IsEmpty());

            if (basketOpt.IsSome)
            {
                session.StartTransaction();
                var newBasket = basketOpt.ValueUnsafe();
                if (newBasket.IsEmpty())
                {
                    await _mongoCustomerBasket.DeleteOneAsync(session, x => x.CustomerId == customerId);
                    await session.CommitTransactionAsync();
                    return FSharpResult<CustomerBasket, Exception>.NewOk(basketOpt.ValueUnsafe());
                }

                await _mongoCustomerBasket.ReplaceOneAsync(session, x => x.CustomerId == customerId,
                    MongoCustomerBasket.MapToMongoCustomerBasket(newBasket));
                await session.CommitTransactionAsync();
                return FSharpResult<CustomerBasket, Exception>.NewOk(basketOpt.ValueUnsafe());
            }

            return FSharpResult<CustomerBasket, Exception>.NewOk(basketOpt.ValueUnsafe());
        }

        public async Task<FSharpResult<CustomerBasket, Exception>> Checkout(Guid customerId)
        {
            using var session = await _client.StartSessionAsync();
            var basket = await _mongoCustomerBasket.Find(session, GetByCustomerIdFilter(customerId))
                .FirstOrDefaultAsync();
            var basketOpt = Optional(basket).Map(MongoCustomerBasket.MapToCustomerBasket);

            if (basketOpt.IsSome)
            {
                session.StartTransaction();
                await _mongoCustomerBasket.DeleteOneAsync(session, x => x.CustomerId == customerId);
                await session.CommitTransactionAsync();
                return FSharpResult<CustomerBasket, Exception>.NewOk(basketOpt.ValueUnsafe());
            }

            return FSharpResult<CustomerBasket, Exception>.NewError(new Exception("No basket to checkout"));
        }

        private Expression<Func<MongoCustomerBasket, bool>> GetByCustomerIdFilter(Guid id)
        {
            return x => x.CustomerId == id;
        }
    }
}