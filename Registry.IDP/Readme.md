## Registry IDP
This is the identity provider project using Identity Server 4. 

### Migrations
To run the migrations, use the following commands,

To setup the configuration tables run the following command,

`dotnet ef database update --context ConfigurationDbContext --startup-project "Registry.IDP"`

To setup the persistent storage tables, run the following command,

`dotnet ef database update --context PersistedgrantDbContext --startup-project "Registry.IDP"`

### Docker

To build with docker, use the folllowing command,

`docker build -t registryidp -f .\Dockerfile ..`

Before creating the container, make sure to update the docker environment settings with the correct connection string for the sql server DB which should be running in another container.

To create the docker container, use the following command.

`docker run -d -e "ASPNETCORE_ENVIRONMENT=Docker" -v "/C/SourceCode/certs:/app/https" -e "SettingsConfig:CertPFXPassword=Password2020" registryidp:latest`