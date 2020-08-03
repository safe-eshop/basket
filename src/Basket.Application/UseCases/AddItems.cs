using System;
using System.Threading.Tasks;
using Basket.Application.Dto;
using Basket.Domain.Repository;
using LanguageExt;
using LanguageExt.Common;

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
            if (request.Item.Quantity <= 0 && request.CustomerId == Guid.Empty)
            {
                return new Result<Unit>(new ArgumentOutOfRangeException("Validation error"));
            }

            var result = await _customerBasketRepository.AddItem(request.CustomerId, request.Item);

            return result.IsError ? new Result<Unit>(result.ErrorValue) : new Result<Unit>(Unit.Default);
        }
    }
}