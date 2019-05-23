#[derive(Debug)]
struct CustomerBasket {
    id: String
}

fn main() {
    let id = String::from("dshjdsaguhdsahgds");
    let basket = CustomerBasket { id };
    println!("b = {:?}", basket)
}
