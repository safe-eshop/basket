mod basket_item;
use basket_item::BasketItem;
use basket_item::Item;
use serde::{Serialize, Deserialize};

type CustomerId = String;

#[derive(Serialize, Deserialize, Debug)]
pub struct CustomerBasket {
    id: CustomerId,
    items: Vec<BasketItem>
}
 
trait Basket {
    fn add_item(&mut self, product_id: basket_item::ProductId, quantity: u8);
    fn remove_item(&mut self, product_id: basket_item::ProductId, quantity: u8);
    fn clear(&mut self);
}

impl CustomerBasket {
    pub fn empty(id: String) -> CustomerBasket {
        return CustomerBasket{ id: id, items: Vec::new()}
    }
}

pub fn get_customer_basket(customer_id: String) -> Result<CustomerBasket, String> {
    let item = BasketItem { id: String::from("test"), quantity: 3 };
    return Ok(CustomerBasket { id: customer_id, items: vec![item] });
}

impl Basket for CustomerBasket {
    fn add_item(&mut self, product_id: basket_item::ProductId, quantity: u8) {
        let position = self.items.iter().position(|item| item.id == product_id);
        match position {
            Some(pos) => {
                self.items[pos].incease_quantity(quantity);      
            },
            None => {
                self.items.push(BasketItem { id: product_id, quantity: quantity});
            }
        }
    }

    fn remove_item(&mut self, product_id: basket_item::ProductId, quantity: u8) {
        let position = self.items.iter().position(|item| item.id == product_id);
        match position {
            Some(pos) => {
                self.items[pos].decrease_quantity(quantity);      
                if self.items[pos].is_empty() {
                    self.items.remove(pos);
                }
            },
            None => {
                panic!("Item not exists");
            }
        }
    }
    
    fn clear(&mut self){
        self.items.clear();
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
        assert_eq!(subject.items.get(0).unwrap().id, String::from("dsddsa"));
        assert_eq!(subject.items.get(0).unwrap().quantity, 24);
        assert_eq!(subject.items.get(1).unwrap().id, String::from("dsddsa22222222222222222222"));
        assert_eq!(subject.items.get(1).unwrap().quantity, 12);
    }  

    #[test]
    #[should_panic]
    fn remove_item_when_no_exists() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.remove_item(String::from("dsddsa"), 12);
    }  

    #[test]
    #[should_panic]
    fn remove_item_when_exists_and_item_quantity_is_smaller() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa22222222222222222222"), 12);
        subject.remove_item(String::from("dsddsa"), 24);
    } 

    #[test]
    fn remove_item_when_exists() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa22222222222222222222"), 12);
        subject.remove_item(String::from("dsddsa"), 12);
        assert_eq!(subject.items.len(), 2);
        assert_eq!(subject.items.get(0).unwrap().id, String::from("dsddsa"));
        assert_eq!(subject.items.get(0).unwrap().quantity, 12);
        assert_eq!(subject.items.get(1).unwrap().id, String::from("dsddsa22222222222222222222"));
        assert_eq!(subject.items.get(1).unwrap().quantity, 12);
    }
    
    #[test]
    fn remove_item_when_exists2() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa22222222222222222222"), 12);
        subject.remove_item(String::from("dsddsa"), 24);
        assert_eq!(subject.items.len(), 1);
        assert_eq!(subject.items.get(0).unwrap().id, String::from("dsddsa22222222222222222222"));
        assert_eq!(subject.items.get(0).unwrap().quantity, 12);
    }    

    #[test]
    fn cclear_basket() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa22222222222222222222"), 12);
        subject.clear();
        assert_eq!(subject.items.len(), 0);
    }    
}