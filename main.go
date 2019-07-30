package main

type BasketItem struct {
	Id int32 `json:"id,omitempty"`
	Quantity int32 `json:"quantity,omitempty"`
}

type CustomerBasket struct {
	Items []BasketItem `json:"items,omitempty"`
	OrderId string 
}



func main()  {
	
}