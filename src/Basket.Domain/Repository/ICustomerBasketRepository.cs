using System.Threading.Tasks;
using Basket.Common.Types;
using Basket.Domain.Model;
using LanguageExt;
using Unit = Basket.Common.Types.Unit;

namespace Basket.Domain.Repository
{
    public interface ICustomerBasketRepository
    {
        Task<RopResult<bool>> CustomerBasketExists(CustomerId id);
        Task<RopResult<CustomerBasket>> GetCustomerBasket(CustomerId id);
        Task<RopResult<Unit>> InserOrUpdate(CustomerBasket basket);
        Task<RopResult<Unit>> Remove(CustomerId id);
    }
}