## Gift Registry

### Summary
This is an example project used to demonstrate the microservices pattern. There are mainly 3 microservices,

1. Product catalog - This service stores and maintains products with a full crud rest API and a GRPC endpoint that can be consumed by other microservices.
2. Reviews - This microservice stores and maintains reviews for products.
3. Registry - This application consumes the product catalog and the reviews microservices and constructs the UI.

The application has been built with,

* .Net core 3.1
* Blazor
* EF Core 3.1
* Grpc

### Running the project
#### Pre-reqs
-  [.Net core SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- LocalDB through the Visual Studio Installer or from [here](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb?view=sql-server-ver15).
- Powershell

#### Migrations
To create the database and seed data, run the following commands from a command prompt window,

First we need to install the dotnet ef core tools package (remember to restart powershell after install),

`dotnet tool install --global dotnet-ef`

Navigate to the root folder through powershell and execute the `SetupDb.ps1` powershell script which sets up all the DBs and sample data.

#### Running the application
After the DBs and the seed data is created, build and run the projects `ProductCatalog.Grpc`, `Reviews.Grpc` and `Registry.UI`.