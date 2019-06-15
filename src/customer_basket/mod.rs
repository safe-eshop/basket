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
        let mut items: Vec<_> = self.items.iter().filter(|&item| item.id == product_id).collect::<Vec<&BasketItem>>();
        let item = items.first_mut();
        match item {
            Some(&mut i) => {
                let a = i;
                println!("sdngdsj")
            },
            None => {
                self.items.push(BasketItem { id: product_id, quantity: quantity })
            }
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
    #[test]
    fn add_item_when_exists() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa"), 12);
        assert_eq!(subject.items.len(), 1);
        assert_eq!(subject.items.first().unwrap().id, String::from("dsddsa"));
        assert_eq!(subject.items.first().unwrap().quantity, 24);
    } 
    #[test]
    fn add_item_when_exists2() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa22222222222222222222"), 12);
        assert_eq!(subject.items.len(), 2);
        assert_eq!(subject.items.first().unwrap().id, String::from("dsddsa"));
        assert_eq!(subject.items.first().unwrap().quantity, 24);
        assert_eq!(subject.items.first().unwrap().id, String::from("dsddsa22222222222222222222"));
        assert_eq!(subject.items.first().unwrap().quantity, 12);
    }  
}