namespace Basket.Domain.Types

open System

type ItemId = Guid
type Quantity = int
type CustomerId = Guid

type Item = { Id: ItemId; Quantity: Quantity } with
    static member Create productId quantity =  { Id = productId; Quantity = quantity }
    member this.IncreaseQuantity(quantity: Quantity) =  { this with Quantity = this.Quantity + quantity }
    member this.DecreaseQuantity(quantity: Quantity) =
        let newQ = this.Quantity - quantity;
        if newQ > 0 then
            { this with Quantity = this.Quantity - quantity }
        else
            this
    
type Items = Item seq
type BasketId = Guid

type CustomerBasket = { Id: BasketId; CustomerId: CustomerId; Items: Items } with
    static member Empty(customerId: CustomerId) = { Id = Guid.NewGuid(); CustomerId = customerId; Items = Seq.empty }
    member this.AddItem(item: Item) =
      let exists = this.Items |> Seq.exists(fun x -> x.Id = item.Id)
      if exists then
          let newItems = this.Items |> Seq.map(fun x -> if x.Id = item.Id then x.IncreaseQuantity(item.Quantity) else x)
          { this with Items = newItems }
      else
        let element = Seq.singleton item
        { this with Items = Seq.append this.Items element }
    member this.RemoveItem(item: Item) = this
        

exception BasketException of id: CustomerId
   