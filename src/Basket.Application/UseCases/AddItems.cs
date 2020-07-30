using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.ApplicationsErrors;
using Basket.Application.Dto;
using Basket.Common.Types;
using Basket.Domain.Model;
using Basket.Domain.Repository;
using LanguageExt;
using Unit = Basket.Common.Types.Unit;
using static LanguageExt.Prelude;

namespace Basket.Application.UseCases
{
    public sealed class AddItems
    {
        private readonly ICustomerBasketRepository _customerBasketRepository;

        public AddItems(ICustomerBasketRepository customerBasketRepository)
        {
            _customerBasketRepository = customerBasketRepository;
        }

        public async Task<Either<Base>> Execute(AddItemsRequest request)
        {
            var items = request.Items?.Select(item => Item.Create(item.productId, item.quantity)).ToList();
            if (items is null)
            {
                return RopResult<Unit>.Failure(new NoItemsToAddApplicationException(request.CustomerId));
            }

            var exists = await _customerBasketRepository.CustomerBasketExists(request.CustomerId);
            var basketResult = await GetBasket(exists, request.CustomerId);
            return await basketResult.Bind(basket => { return basket.AddItems(items).Map(_ => basket); })
                .BindAsync(
                    async basket => await _customerBasketRepository.InserOrUpdate(basket));
        }

        private async Task<RopResult<CustomerBasket>> GetBasket(RopResult<bool> exists, CustomerId customerId)
        {
            return await exists.BindAsync(async exist =>
            {
                if (!exist) return RopResult<CustomerBasket>.Ok(CustomerBasket.Empty(customerId));

                return await _customerBasketRepository.GetCustomerBasket(customerId);
            });
        }
    }
}