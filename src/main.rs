
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

fn main() {
    let item = BasketItem { id: String::from("test"), quantity: 3 };
    let basket = CustomerBasket { id: String::from("dshjdsaguhdsahgds"), items: vec![item] };
    println!("b = {:?}", basket)
}
