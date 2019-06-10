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

fn get_customer_basket(customer_id: String) -> Result<CustomerBasket, String> {
    let item = BasketItem { id: String::from("test"), quantity: 3 };
    return Ok(CustomerBasket { id: customer_id, items: vec![item] });
}

fn main() {
    let basket = get_customer_basket(String::from("ghasdggadsjhgds"));
    match basket {
        Ok(mut b) => {
            // b.add_item(String::from("te3st"), 12);
            println!("Ok {:?}", b);
        }           
        Err(er) =>  println!("Error {}", er)
    }
}
