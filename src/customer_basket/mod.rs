pub mod basket_item;
use basket_item::BasketItem;
use serde::{Serialize, Deserialize};

type CustomerId = String;

#[derive(Serialize, Deserialize, Debug)]
pub struct CustomerBasket {
    id: CustomerId,
    items: Vec<BasketItem>
}
 
pub trait Basket {
}

impl CustomerBasket {
    pub fn empty(id: String) -> CustomerBasket {
        return CustomerBasket{ id: id, items: Vec::new()}
    }

    pub fn change_item_quantity(&mut self, product_id: basket_item::ProductId, quantity: i8) -> Result<(), &str> {
        if quantity > 0 {
            self.add_item(product_id, quantity as u8);
            Ok(())
        } else {
            let abs: u8 = (quantity * -1) as u8;
            self.remove_item(product_id, abs)
        }
    }

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

    fn remove_item(&mut self, product_id: basket_item::ProductId, quantity: u8) -> Result<(), &str> {
        let position = self.items.iter().position(|item| item.id == product_id);
        match position {
            Some(pos) => {
                let element = &mut self.items[pos];
                if element.quantity < quantity {
                    return Err("To small quantity");
                } else {
                    element.quantity -= quantity;
                    if element.is_empty() {
                        self.items.remove(pos);                   
                    }  
                    return Ok(());
                }     
            },
            None => {
                Err("Item not exists")
            }
        }
    }
    
    pub fn clear(&mut self) {
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
    fn remove_item_when_no_exists() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        let res = subject.remove_item(String::from("dsddsa"), 12);
        assert!(res.is_err());
        assert_eq!(res, Err("Item not exists"))
    }  

    #[test]
    fn remove_item_when_exists_and_item_quantity_is_smaller() {
        let mut subject = CustomerBasket::empty(String::from("dskjhdsghdsfkjh"));
        subject.add_item(String::from("dsddsa"), 12);
        subject.add_item(String::from("dsddsa22222222222222222222"), 12);
        let res = subject.remove_item(String::from("dsddsa"), 24);
        assert!(res.is_err());
        assert_eq!(res, Err("To small quantity"))
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