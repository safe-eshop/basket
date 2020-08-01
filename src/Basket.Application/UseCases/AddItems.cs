using System;
using System.Linq;
using System.Threading.Tasks;
using Basket.Application.ApplicationsErrors;
using Basket.Application.Dto;
using Basket.Domain.Repository;
using Basket.Domain.Types;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using static LanguageExt.FSharp;

namespace Basket.Application.UseCases
{
    public sealed class AddItem
    {
        private readonly ICustomerBasketRepository _customerBasketRepository;

        public AddItem(ICustomerBasketRepository customerBasketRepository)
        {
            _customerBasketRepository = customerBasketRepository;
        }

        public async Task<Result<Unit>> Execute(AddItemRequest request)
        {
            await _customerBasketRepository.StartTransaction();
            var basketResult = await GetBasket(request.CustomerId);
            var newBasket = basketResult.AddItem(request.Item);

            var result = await _customerBasketRepository.InsertOrUpdate(newBasket);

            if (result.IsError)
            {
                 await _customerBasketRepository.AbortTransaction();
                 return new Result<Unit>(result.ErrorValue);
            }

            await _customerBasketRepository.CompleteTransaction();
            return new Result<Unit>(Unit.Default);
        }

        private async Task<CustomerBasket> GetBasket(Guid customerId)
        {
            var result = await _customerBasketRepository.Get(customerId);
            return fs(result).IfNone(() => CustomerBasket.Empty(customerId));
        }
    }
}