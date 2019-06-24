extern crate redis;
use redis::Commands;
use actix_web::{web, middleware, App, HttpServer, Responder};

fn index(info: web::Path<String>) -> impl Responder {
    let client = redis::Client::open("redis://redis/").unwrap();
    let con = client.get_connection().unwrap();
    // throw away the result, just make sure it does not fail
    let _ : () = con.set("my_key", 42).unwrap();
    // read back the key and return it.  Because the return value
    // from the function is a result for integer this will automatically
    // convert into one.
    let a = con.get("my_key").unwrap_or(0i32);
    format!("Hello {}! id from redis = {}", *info, a)
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
              web::resource("/hello/{name}").to(index)))
        .bind("127.0.0.1:8080")?
        .start();
    println!("Starting http server: 127.0.0.1:8080");
    sys.run()
}