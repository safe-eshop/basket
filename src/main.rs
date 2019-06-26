extern crate redis;
mod customer_basket;
use redis::Commands;
use actix_web::{web, middleware, App, Error, error, HttpResponse, HttpServer};
use customer_basket::{CustomerBasket};
use customer_basket::basket_item::BasketItem;
use futures::{future, Future, Stream};

static CLIENT_ID: &str = "4cba04f1-2436-4d79-b184-b1a8521ff0f9";

fn redis() -> redis::Connection {
    let client = redis::Client::open("redis://redis/").unwrap();
    return client.get_connection().unwrap();
}

fn index(info: web::Path<String>) -> impl Future<Item = HttpResponse, Error = Error> {
    let client = redis::Client::open("redis://redis/").unwrap();
    let con = client.get_connection().unwrap();
    // throw away the result, just make sure it does not fail
    let _ : () = con.set("my_key", 42).unwrap();

    future::result::<i32, redis::RedisError>(con.get("my_key"))        
        .then(|res| {
            match res {
                Ok(val) => Ok(val + 3),
                Err(e) => {
                    println!("Error {}", e);
                    Err(error::ErrorBadRequest("redis error"))
                },
            }
        })
        .and_then(|body| {
            Ok(HttpResponse::Ok().json(body)) // <- send response
        })
}

fn get_customer_basket() -> impl Future<Item = HttpResponse, Error = Error>  {
    let con = redis();
    future::result::<Option<String>, redis::RedisError>(con.get(CLIENT_ID))        
        .then(|customer_basket| {
            match customer_basket {
                Ok(basket) => {
                    let basket_resp = basket.map_or(CustomerBasket::empty(String::from(CLIENT_ID)), |v| serde_json::from_str(&v).unwrap());
                    Ok(basket_resp)
                }
                Err(ex) => {
                    println!("Error {}", ex);
                    Err(error::ErrorBadRequest("redis error"))
                }
            } 
        })
        .and_then(|body| {
            Ok(HttpResponse::Ok().json(body)) // <- send response
        }) 
}

fn add_item(item: web::Json<BasketItem>,) -> impl Future<Item = HttpResponse, Error = Error> {
    let con = redis();
    future::result::<Option<String>, redis::RedisError>(con.get(CLIENT_ID))        
        .then(|customer_basket| {
            match customer_basket {
                Ok(basket) => {
                    let mut basket_resp = basket.map_or(CustomerBasket::empty(String::from(CLIENT_ID)), |v| serde_json::from_str(&v).unwrap());                   
                    basket_resp.add_item(String::from("a971a2de-1d52-46d9-a9aa-e6df2e072d46"), 10);
                    let _ : () = redis().set(CLIENT_ID, serde_json::to_string(&basket_resp)?).unwrap();
                    Ok(basket_resp)
                }
                Err(ex) => {
                    println!("Error {}", ex);
                    Err(error::ErrorBadRequest("redis error"))
                }
            } 
        })
        .and_then(|body| {
            Ok(HttpResponse::Ok().json(body)) // <- send response
        }) 
}

fn main() -> std::io::Result<()> {
    std::env::set_var("RUST_LOG", "actix_web=info");
    env_logger::init();
    println!("Server starting");
    let sys = actix_rt::System::new("basket-api");
    HttpServer::new(
        || App::new()
        .wrap(middleware::Logger::default())
        .data(web::JsonConfig::default().limit(4096)) 
        .service(web::resource("/").to_async(get_customer_basket))
        .service(
              web::resource("/hello/{name}").to_async(index))
        .service(web::resource("/item").route(web::post().to_async(add_item))))
        .bind("127.0.0.1:8080")?
        .start();
    println!("Starting http server: 127.0.0.1:8080");
    sys.run()
}