using Basket.Domain.Model;

namespace Basket.Application.Dto
{
    public class GetCustomerBasketRequest
    {
        public GetCustomerBasketRequest(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        public CustomerId CustomerId { get; }
    }
}