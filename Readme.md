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
- OpenSSL - Download the installer from [here](https://slproweb.com/products/Win32OpenSSL.html) and make sure the openssl executable is added to your system path.
- Docker (optional) - Download and install [docker desktop](https://www.docker.com/products/docker-desktop). 

#### Migrations
To create the database and seed data, run the following commands from a command prompt window,

First we need to install the dotnet ef core tools package (remember to restart powershell after install),

`dotnet tool install --global dotnet-ef`

Navigate to the root folder through powershell and execute the `SetupDb.ps1` powershell script which sets up all the DBs and sample data.

#### TLS
The project uses self signed certificates. To generate these certificates, run the following commands in a folder from a powershell window,

First we generate a private key,

`openssl genrsa -des3 -out myCA.key 2048`

Next we use this to generate a root certificate,

`openssl req -x509 -new -nodes -key myCA.key -sha256 -days 825 -out myCA.pem`

We can now create and get a certificate signed. First we start with a private key,

`openssl.exe genrsa -out localhost.key 2048`

Create a certificate signing request,

`openssl req -new -key localhost.key -out localhost.csr`

Create a configuration file with some preset values that will be used to get our certificate. We can call this file localhost.ext and populate it with the following contents,

`authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
keyUsage = digitalSignature, nonRepudiation, keyEncipherment, dataEncipherment
subjectAltName = @alt_names
[alt_names]	
DNS.1 = localhost`

Now we can generate our certificate,

`openssl x509 -req -in localhost.csr -CA myCA.pem -CAkey myCA.key -CAcreateserial -out localhost.crt -days 825 -sha256 -extfile localhost.ext`

The last step to place the private key along side the certificate in a pfx file,

`openssl pkcs12 -export -in localhost.crt -inkey localhost.key -out localhost.pfx`

When running with visual studio,

1. The myCA.pem file needs to be added to the Trusted Root Certification Authority. 
2. Create a secret with the path "HttpServer:Https:Password=Paste_Cert_Password_here"

When running with docker,

1. First build the image for each project,

`docker build -t registryidp -f .\Dockerfile ..`

2. Run the container,

`docker run -d -e "ASPNETCORE_ENVIRONMENT=Docker" -p 8001:8001 -p 8000:8000 -v "/C/SourceCode/certs:/app/https" -e "HttpServer:Https:Password=Paste_Cert_Password_here" registryidp:latest`

#### Running the application
After the DBs and the seed data is created, the projects can be run from visual stutio or from docker. From VS, build and run the projects `ProductCatalog.Grpc`, `Reviews.Grpc` and `Registry.UI`.
