using System.Threading.Tasks;
using Basket.Common.Types;
using Basket.Domain.Model;
using Basket.Domain.Repository;
using Basket.Infrastructure.InfrastructureExceptions;
using LanguageExt;
using Unit = Basket.Common.Types.Unit;

namespace Basket.ApplicationUnitTests.Infrastructure
{
    public class FakeCustomerBasketRepository : ICustomerBasketRepository
    {
        private readonly CustomerId _realcustomer;
        private bool _shouldExist;
        private bool _sholdBeError;
        
        public FakeCustomerBasketRepository(CustomerId realcustomer, bool shouldExist, bool sholdBeError)
        {
            _realcustomer = realcustomer;
            _shouldExist = shouldExist;
            _sholdBeError = sholdBeError;
        }
        public async Task<RopResult<bool>> CustomerBasketExists(CustomerId id)
        {
            if (_sholdBeError)
            {
                return RopResult<bool>.Failure(new BasketRedisException(id));
            }
            if (_shouldExist)
            {
                return RopResult<bool>.Ok(false);
            }
            return RopResult<bool>.Ok(true);
        }

        public Task<RopResult<Option<CustomerBasket>>> GetCustomerBasket(CustomerId id)
        {
            throw new System.NotImplementedException();
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