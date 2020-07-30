namespace Basket.Domain.Repository
open System.Threading.Tasks
open Basket.Domain.Types

type ICustomerBasketRepository =
    abstract member Get: CustomerId -> Task<CustomerBasket option>
    abstract member Exists: CustomerId -> Task<bool>