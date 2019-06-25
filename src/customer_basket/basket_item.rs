use serde::{Serialize, Deserialize};

pub type ProductId = String;

#[derive(Serialize, Deserialize, Debug)]
pub struct BasketItem {
    pub id: ProductId,
    pub quantity: u8
}

pub trait Item {
    fn incease_quantity(&mut self, q: u8);
    fn decrease_quantity(&mut self, q: u8);
    fn is_empty(&self) -> bool;
}

impl BasketItem {
    fn empty(id: ProductId) -> BasketItem {
        BasketItem { id: id, quantity: 0 }
    }
    fn new(id: ProductId, quantity: u8) -> BasketItem {
        BasketItem { id: id, quantity: quantity }
    }
}

impl Item for BasketItem {
    fn incease_quantity(&mut self, q: u8) {
        self.quantity += q;
    }
    fn decrease_quantity(&mut self, q: u8) {
        if self.quantity < q {
            panic!("To small quantity");
        } else {
            self.quantity -= q;
        }
    }
    fn is_empty(&self) -> bool {
        return self.quantity == 0;
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    #[should_panic]
    fn test_decrease_item_quantity_when_is_to_small() {
        BasketItem::empty(String::from("fdsajhgdsaj")).decrease_quantity(1);
    }  

    #[test]
    fn test_decrease_item_quantity() {
        let mut subject = BasketItem::new(String::from("fdsajhgdsaj"), 2);
        subject.decrease_quantity(1);
        assert_eq!(subject.quantity, 1)
    } 

    #[test]
    fn test_increase_item_quantity() {
        let mut subject = BasketItem::new(String::from("fdsajhgdsaj"), 2);
        subject.incease_quantity(1);
        assert_eq!(subject.quantity, 3)
    }       
}