using System.Linq;
using Basket.Application.Dto;
using Basket.Domain.Types;

namespace Basket.Application.Mappers
{
    public static class CustomerBasketToDto
    {
        public static CustomerBasketDto ToDto(this CustomerBasket customerBasket)
        {
            return new CustomerBasketDto(customerBasket.Id, customerBasket.CustomerId,
                customerBasket.Items.Select(item => item.ToDto()).ToList());
        }

        public static ItemDto ToDto(this Item item)
        {
            return new ItemDto(item.Id, item.Quantity);
        }
    }
}