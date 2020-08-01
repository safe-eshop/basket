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

namespace Basket.Infrastructure.Repository
{
    public class MongoCustomerBasketRepository : ICustomerBasketRepository, IDisposable
    {
        public const string CustomerBasketCollection = nameof(CustomerBasket);
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<MongoCustomerBasket> _mongoCustomerBasket;
        private Option<IClientSessionHandle> _session;

        public MongoCustomerBasketRepository(IMongoClient client, IMongoDatabase database)
        {
            _client = client;
            _database = database;
            _mongoCustomerBasket = _database.GetCollection<MongoCustomerBasket>(CustomerBasketCollection);
            _session = None;
        }

        public async Task<FSharpOption<CustomerBasket>> Get(Guid customerBasketId)
        {
            var query = _session
                .Match(session => _mongoCustomerBasket.Find(session, Filter(customerBasketId)),
                    () => _mongoCustomerBasket.Find(Filter(customerBasketId)));
            var collection = await query.FirstOrDefaultAsync();
            return Optional(collection).Map(MongoCustomerBasket.MapToCustomerBasket).ToFSharp();

            Expression<Func<MongoCustomerBasket, bool>> Filter(Guid id)
            {
                return x => x.CustomerId == id;
            }
        }

        public async Task<FSharpResult<Unit, Exception>> InsertOrUpdate(CustomerBasket customerBasket)
        {
            var items = customerBasket.Items
                .Select(x => new MongoCustomerBasketItem() {ItemId = x.Id, Quantity = x.Quantity}).ToList();
            var mongo = new MongoCustomerBasket()
            {
                Id = customerBasket.Id,
                CustomerId = customerBasket.CustomerId,
                Items = items
            };
            var filter = Builders<MongoCustomerBasket>.Filter.Eq(x => x.Id, customerBasket.Id);
            var options = new ReplaceOptions {IsUpsert = true};
            return await _session.MatchAsync(async session =>
            {
                await _mongoCustomerBasket.ReplaceOneAsync(session, filter, mongo, options);
                return Domain.Result.UnitOk<Exception>();
            }, async () =>
            {
                await _mongoCustomerBasket.ReplaceOneAsync(filter, mongo, options);
                return Domain.Result.UnitOk<Exception>();
            });
        }

        public async Task<bool> Exists(Guid basketId)
        {
            throw new NotImplementedException();
        }

        public async ValueTask StartTransaction()
        {
            var ses = await _client.StartSessionAsync();
            _session = Some(ses);
        }

        public async ValueTask CompleteTransaction()
        {
            await _session.MatchAsync(async session => await session.CommitTransactionAsync(),
                () => throw new Exception("No session started"));
        }

        public async ValueTask AbortTransaction()
        {
            await _session.MatchAsync(async session => await session.AbortTransactionAsync(),
                () => throw new Exception("No session started"));
        }


        public void Dispose()
        {
            _session.IfSome(session =>
            {
                session.Dispose();
            });
        }
    }
}