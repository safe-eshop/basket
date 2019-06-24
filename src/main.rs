use actix_web::{web, middleware, App, HttpServer, Responder};

fn index(info: web::Path<String>) -> impl Responder {
    format!("Hello {}!", *info)
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