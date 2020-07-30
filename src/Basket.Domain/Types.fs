namespace Basket.Domain.Types

open System

type ItemId = Guid
type Quantity = int
type CustomerId = Guid

type Item = { Id: ItemId; Quantity: Quantity } with
    static member Create productId quantity =  { Id = productId; Quantity = quantity }
    
type Items = Item seq

type CustomerBasket = { CustomerId: CustomerId; Items: Items } with
    static member Empty(customerId: CustomerId) = { CustomerId = customerId; Items = Seq.empty }
    member this.AddItems(items: Items) =
        this
        

exception BasketException of id: CustomerId
   