using System.Threading.Tasks;
using Basket.Application.Dto;
using Basket.Common.Types;
using Basket.Domain.Model;
using Basket.Domain.Repository;
using LanguageExt;
using Basket.Application.Mappers;
using static LanguageExt.Prelude;
namespace Basket.Application.UseCases
{
    public class GetCustomerBasket
    {
        private readonly ICustomerBasketRepository _customerBasketRepository;

        public GetCustomerBasket(ICustomerBasketRepository customerBasketRepository)
        {
            _customerBasketRepository = customerBasketRepository;
        }
        
        public async Task<RopResult<CustomerBasketDto>> Execute(GetCustomerBasketRequest request)
        {
            var exists = await _customerBasketRepository.CustomerBasketExists(request.CustomerId);
            var basket = await exists.BindAsync(async exist =>
            {
                if (exist)
                {
                    return await _customerBasketRepository.GetCustomerBasket(request.CustomerId);
                }

                return RopResult<CustomerBasket>.Ok(CustomerBasket.Empty(request.CustomerId));
            });
            return basket.Map(b => b.ToDto());
        }
    }
}