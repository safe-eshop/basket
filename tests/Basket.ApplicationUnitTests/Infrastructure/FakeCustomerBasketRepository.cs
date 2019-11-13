using System.Threading.Tasks;
using Basket.Common.Types;
using Basket.Domain.Model;
using Basket.Domain.Repository;
using Basket.Infrastructure.InfrastructureExceptions;
using LanguageExt;
using Unit = Basket.Common.Types.Unit;
using static LanguageExt.Prelude;

namespace Basket.ApplicationUnitTests.Infrastructure
{
    public class FakeCustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly CustomerId _realcustomer;
        private bool _shouldExist;
        private bool _sholdBeError;
        private Option<CustomerBasket> _feed;
        
        
        public FakeCustomerBasketRepository(CustomerId realcustomer, bool shouldExist, bool sholdBeError) : this(realcustomer, shouldExist, sholdBeError, None)
        {
        }
        
        public FakeCustomerBasketRepository(CustomerId realcustomer, bool shouldExist, bool sholdBeError, Option<CustomerBasket> feed)
        {
            _realcustomer = realcustomer;
            _shouldExist = shouldExist;
            _sholdBeError = sholdBeError;
            _feed = feed;
        }
        public async Task<RopResult<bool>> CustomerBasketExists(CustomerId id)
        {
            if (_sholdBeError)
            {
                return RopResult<bool>.Failure(new BasketRedisException(id));
            }
            if (_shouldExist)
            {
                return RopResult<bool>.Ok(true);
            }
            return RopResult<bool>.Ok(false);
        }

        public async Task<RopResult<CustomerBasket>> GetCustomerBasket(CustomerId id)
        {
            if (_sholdBeError)
            {
                return RopResult<CustomerBasket>.Failure(new BasketRedisException(id));
            }
            
            return RopResult<CustomerBasket>.Ok(_feed.IfNone(CustomerBasket.Empty(id)));
        }

        public Task<RopResult<Unit>> InserOrUpdate(CustomerBasket basket)
        {
            throw new System.NotImplementedException();
        }

        public Task<RopResult<Unit>> Remove(CustomerId id)
        {
            throw new System.NotImplementedException();
        }
    }
}