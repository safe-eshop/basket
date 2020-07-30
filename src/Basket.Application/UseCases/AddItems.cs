using System;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.ApplicationsErrors;
using Basket.Application.Dto;
using Basket.Domain.Repository;
using Basket.Domain.Types;
using LanguageExt;
using static LanguageExt.Prelude;
using static LanguageExt.FSharp;
using FSharp = LanguageExt.FSharp;

namespace Basket.Application.UseCases
{
    public sealed class AddItems
    {
        private readonly ICustomerBasketRepository _customerBasketRepository;

        public AddItems(ICustomerBasketRepository customerBasketRepository)
        {
            _customerBasketRepository = customerBasketRepository;
        }

        public async Task<Either<Exception, Unit>> Execute(AddItemsRequest request)
        {
            var items = request.Items?.Select(item => Item.Create(item.productId, item.quantity)).ToList();
            if (items is null)
            {
                return Left<Exception, Unit>(new NoItemsToAddApplicationException(request.CustomerId));
            }
            
            var basketResult = await GetBasket(request.CustomerId);
            return await basketResult.Bind(basket => { return basket.AddItems(items).Map(_ => basket); })
                .BindAsync(
                    async basket => await _customerBasketRepository.InserOrUpdate(basket));
        }

        private async Task<Either<Exception, CustomerBasket>> GetBasket(Guid customerId)
        {
            var result = await _customerBasketRepository.Get(customerId);
            return fs(result).Match(Right<Exception, CustomerBasket>,
                () => Right<Exception, CustomerBasket>(CustomerBasket.Empty(customerId)));
        }
    }
}