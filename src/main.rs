mod customer_basket;
use customer_basket::get_customer_basket;

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
