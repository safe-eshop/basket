namespace Basket.Domain.Repository
open System.Threading.Tasks
open Basket.Domain.Types

type ICustomerBasketRepository =
    abstract member CustomerBasketExists: CustomerId -> Task<CustomerBasket option>