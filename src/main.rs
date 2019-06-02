
type ProductId = String;

#[derive(Debug)]
struct BasketItem {
    id: ProductId,
    quantity: u8
}

type CustomerId = String;

#[derive(Debug)]
struct CustomerBasket {
    id: CustomerId,
    items: Vec<BasketItem>
}
 
trait Basket {
    fn add_item(&mut self, product_id: ProductId, quantity: u8);
}

impl CustomerBasket {
    fn new(id: String) -> CustomerBasket {
        return CustomerBasket{ id: id, items: Vec::new()}
    }
}

impl Basket for CustomerBasket {
    fn add_item(&mut self, product_id: ProductId, quantity: u8)  {
        let items: Vec<_> = self.items.iter().filter(|&item| item.id == product_id).collect();
        let item = items.first();
        match item {
            Some(i) => {
                let newQuantity = i.quantity + quantity;
                let newItem = BasketItem{ quantity: newQuantity, ..i };
                println!("{:?}", i)
            }
            None => println!("Nope")
        }
        
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
            b.add_item(String::from("test"), 12);
            println!("Ok {:?}", b);
        }           
        Err(er) =>  println!("Error {}", er)
    }
}
