extern crate redis;
mod customer_basket;
use redis::Commands;
use actix_web::{web, middleware, App, Error, HttpResponse, HttpServer};
use customer_basket::{CustomerBasket};
use futures::{future, Future, Stream};

fn redis() -> redis::Connection {
    let client = redis::Client::open("redis://redis/").unwrap();
    return client.get_connection().unwrap();
}

fn index(info: web::Path<String>) -> impl Future<Item = HttpResponse, Error = Error> {
    let client = redis::Client::open("redis://redis/").unwrap();
    let con = client.get_connection().unwrap();
    // throw away the result, just make sure it does not fail
    let _ : () = con.set("my_key", 42).unwrap();
    // read back the key and return it.  Because the return value
    // from the function is a result for integer this will automatically
    // convert into one.
    let a = con.get("my_key");

    future::result::<i32, redis::RedisError>(con.get("my_key"))        
        .then(|res| {
          match res {
              Ok(val) => Ok(val + 3),
              Err(e) => Err(e),
          }
        });
}

fn get_customer_basket() -> HttpResponse  {
    let con = redis();
    let id = "4cba04f1-2436-4d79-b184-b1a8521ff0f9";
    // convert into one.
    let customer_basket: Result<Option<String>, redis::RedisError> = con.get(id);
    match customer_basket {
        Ok(basket) => {
            let basket_resp = basket.map_or(CustomerBasket::empty(String::from(id)), |v| serde_json::from_str(&v).unwrap());
            HttpResponse::Ok().json(basket_resp)
        }
        Err(ex) => {
            println!("Error {}", ex);
            HttpResponse::Ok().json(CustomerBasket::empty(String::from(id)))
        }
    }   
}

fn main() -> std::io::Result<()> {
    std::env::set_var("RUST_LOG", "actix_web=info");
    env_logger::init();
    println!("Server starting");
    let sys = actix_rt::System::new("basket-api");
    HttpServer::new(
        || App::new()
        .wrap(middleware::Logger::default())
        .service(
              web::resource("/hello/{name}").to_async(index))
        .service(web::resource("/").to(get_customer_basket)))
        .bind("127.0.0.1:8080")?
        .start();
    println!("Starting http server: 127.0.0.1:8080");
    sys.run()
}