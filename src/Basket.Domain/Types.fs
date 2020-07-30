namespace Basket.Domain.Types

open System

type ItemId = Guid
type Quantity = int
type CustomerId = Guid

type Item = { Id: ItemId; Quantity: Quantity }
type Items = Item seq

type CustomerBasket = { CustomerId: CustomerId; Items: Items }

type BasketError = int