mod basket_item;
use basket_item::BasketItem;

type CustomerId = String;

#[derive(Debug)]
struct CustomerBasket {
    id: CustomerId,
    items: Vec<BasketItem>
}
 
trait Basket {
    fn add_item(&mut self, product_id: basket_item::ProductId, quantity: u8);
}

impl CustomerBasket {
    fn empty(id: String) -> CustomerBasket {
        return CustomerBasket{ id: id, items: Vec::new()}
    }
}