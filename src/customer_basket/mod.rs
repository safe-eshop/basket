mod basket_item;
use basket_item::BasketItem;

type CustomerId = String;

#[derive(Debug)]
pub struct CustomerBasket {
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

pub fn get_customer_basket(customer_id: String) -> Result<CustomerBasket, String> {
    let item = BasketItem { id: String::from("test"), quantity: 3 };
    return Ok(CustomerBasket { id: customer_id, items: vec![item] });
}

impl Basket for CustomerBasket {
    fn add_item(&mut self, product_id: basket_item::ProductId, quantity: u8) {
        let items: Vec<_> = self.items.iter().filter(|item| item.id == product_id).collect();
        let item = items.first();
        match item {
            Some(_) => {
                println!("sdngdsj")
            },
            None => println!("dsadsasad")
        }
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    #[test]
    fn add_item_when_no_exists() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        assert_eq!(subject.items.len(), 1);
        assert_eq!(subject.items.first().unwrap().id, String::from("dsddsa"));
        assert_eq!(subject.items.first().unwrap().quantity, 12);
    }  
}