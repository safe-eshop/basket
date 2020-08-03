namespace Basket.Domain.Repository
open System
open System.Threading.Tasks
open Basket.Domain.Types
    
type ICustomerBasketRepository =
    abstract member GetByCustomerId: customerId: CustomerId -> Task<CustomerBasket option>
    abstract member AddItem: customerId: CustomerId * item: Item  -> Task<Result<CustomerBasket, Exception>>
    abstract member RemoveItem: customerId: CustomerId * item: Item -> Task<Result<CustomerBasket, Exception>>
    abstract member Checkout: customerId: CustomerId -> Task<Result<CustomerBasket, Exception>>
    
    
