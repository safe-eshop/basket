using System;

namespace Basket.Application.Dto
{
    public class GetCustomerBasketRequest
    {
        public GetCustomerBasketRequest(Guid customerId)
        {
            CustomerId = customerId;
        }

        public Guid CustomerId { get; }
    }
}