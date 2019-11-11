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
        private ICustomerBasketRepository _customerBasketRepository;

        public AddItems(ICustomerBasketRepository customerBasketRepository)
        {
            this.customerBasketRepository = customerBasketRepository;
        }

        public async Task<RopResult<Unit>> Execute(AddItemsRequest request)
        {
            var customerId = new CustomerId(request.CustomerId);
            var items = request.Items?.Select(item => Item.Create(item.productId, item.quantity)).ToList();
            if (items is null)
            {
                 return RopResult<Unit>.Failure(new NoItemsToAddException(customerId));
            }
            
            var exists = await _customerBasketRepository.CustomerBasketExists(customerId);
            var basketResult = await GetBasket(exists, customerId);
            var addItemsResult = await basketResult.BindAsync(basket =>
            {
                var newBasket = basket.AddItems(items);
                return newBasket;
            })
        }

        private async Task<RopResult<CustomerBasket>> GetBasket(RopResult<bool> exists, CustomerId customerId)
        {
            return await exists.BindAsync(async exist =>
            {
                if (!exist) return RopResult<CustomerBasket>.Ok(CustomerBasket.Empty(customerId));
                
                var result = await _customerBasketRepository.GetCustomerBasket(customerId);
                return result.Map(b => b.IfNone(() => CustomerBasket.Empty(customerId)));
            });
        }

        private RopResult<Unit> CheckRequest(CustomerId customerId, AddItemsRequest request)
        {
            if (!request?.Items?.Any() ?? false)
            {
                return RopResult<Unit>.Failure(new NoItemsToAddException(customerId));
            }
            
        }
    }
}