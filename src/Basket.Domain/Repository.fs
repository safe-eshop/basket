namespace Basket.Domain.Repository
open System
open System.Threading.Tasks
open Basket.Domain.Types

type ICustomerBasketRepository =
    abstract member Get: customerId: CustomerId -> Task<CustomerBasket option>
    abstract member InsertOrUpdate: customerBasket: CustomerBasket -> Task<Result<unit, Exception>>
    abstract member Exists: customerId: CustomerId -> Task<bool>
    abstract member Remove: customerId: CustomerId -> Task<Result<unit, Exception>>
    abstract member StartTransaction: unit -> ValueTask
    abstract member CompleteTransaction: unit -> ValueTask
    abstract member AbortTransaction: unit -> ValueTask