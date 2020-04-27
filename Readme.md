## Gift Registry

### Summary
This is an example project used to demonstrate the microservices pattern. There are mainly 3 microservices,

1. Product catalog - This service that stores and maintains products with a full crud rest API and a GRPC endpoint that can be consumed by other microservices.
2. Reviews - This microservice stores and maintains reviews for products.
3. Registry - This application consumes the product catalog and the reviews microservices and constructs the UI.

The application has been built with,

* .Net core
* Blazor (Devexpress for controls)
* EF Core
* Grpc

### Running the project
Run `ProductCatalog.Grpc`, `Reviews.Grpc` and `Registry.UI`.
