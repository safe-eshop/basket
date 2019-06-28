extern crate redis;
mod customer_basket;
use redis::Commands;
use actix_web::{web, middleware, App, Error, error, HttpResponse, HttpServer};
use customer_basket::{CustomerBasket};
use customer_basket::basket_item::ProductId;
use futures::{future, Future};
use serde::{Serialize, Deserialize};

static CLIENT_ID: &str = "4cba04f1-2436-4d79-b184-b1a8521ff0f9";


#[derive(Serialize, Deserialize, Debug)]
pub struct ChangeBasketItemQuantityRequest {
    pub id: ProductId,
    pub quantity: i8
}

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

fn change_item_quantity(item: web::Json<ChangeBasketItemQuantityRequest>,) -> impl Future<Item = HttpResponse, Error = Error> {
    let con = redis();
    future::result::<Option<String>, redis::RedisError>(con.get(CLIENT_ID))        
        .then(move |customer_basket| {
            match customer_basket {
                Ok(basket) => {
                    let con = con;
                    let id = item.id.to_owned();
                    let q = item.quantity;
                    let mut basket_resp = basket.map_or(CustomerBasket::empty(String::from(CLIENT_ID)), |v| serde_json::from_str(&v).unwrap());                   
                    basket_resp.change_item_quantity(id, q);
                    let _ : () = con.set(CLIENT_ID, serde_json::to_string(&basket_resp)?).unwrap();
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


fn clear_basket() -> impl Future<Item = HttpResponse, Error = Error> {
    let con = redis();
    future::result::<Option<String>, redis::RedisError>(con.get(CLIENT_ID))        
        .then(move |customer_basket| {
            match customer_basket {
                Ok(basket) => {
                    let con = con;
                    let mut basket_resp = basket.map_or(CustomerBasket::empty(String::from(CLIENT_ID)), |v| serde_json::from_str(&v).unwrap());                   
                    basket_resp.clear();
                    let _ : () = con.set(CLIENT_ID, serde_json::to_string(&basket_resp)?).unwrap();
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
        .service(web::resource("/items").route(web::post().to_async(change_item_quantity)).route(web::delete().to_async(clear_basket))))
        .bind("127.0.0.1:8080")?
        .start();
    println!("Starting http server: 127.0.0.1:8080");
    sys.run()
}