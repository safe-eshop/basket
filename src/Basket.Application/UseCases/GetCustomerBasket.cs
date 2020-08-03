using System.Threading.Tasks;
using Basket.Application.Dto;
using Basket.Domain.Repository;
using LanguageExt;
using Basket.Application.Mappers;
using static LanguageExt.Prelude;
using static LanguageExt.FSharp;

namespace Basket.Application.UseCases
{
    public class GetCustomerBasket
    {
        private readonly ICustomerBasketRepository _customerBasketRepository;

        public GetCustomerBasket(ICustomerBasketRepository customerBasketRepository)
        {
            _customerBasketRepository = customerBasketRepository;
        }
        
        public async Task<Option<CustomerBasketDto>> Execute(GetCustomerBasketRequest request)
        {
            var result = await _customerBasketRepository.GetByCustomerId(request.CustomerId);
            return fs(result).Map(b => b.ToDto());
        }
    }
}