namespace Basket.Domain.Repository
open System
open System.Threading.Tasks
open Basket.Domain.Types

type ICustomerBasketRepository =
    abstract member Get: CustomerId -> Task<CustomerBasket option>
    abstract member InsertOrUpdate: CustomerBasket -> Task<Result<unit, Exception>>
    abstract member Exists: CustomerId -> Task<bool>
    abstract member StartTransaction: unit -> ValueTask
    abstract member CompleteTransaction: unit -> ValueTask
    abstract member AbortTransaction: unit -> ValueTask