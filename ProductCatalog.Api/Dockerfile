#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5010
EXPOSE 5011

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ProductCatalog.Api/ProductCatalog.Api.csproj", "ProductCatalog.Api/"]
COPY ["ProductCatalog.Repositories/ProductCatalog.Repository.csproj", "ProductCatalog.Repositories/"]
COPY ["ProductCatalog.Models/ProductCatalog.Models.csproj", "ProductCatalog.Models/"]
COPY ["ProductCatalog.Shared/ProductCatalog.Shared.csproj", "ProductCatalog.Shared/"]
RUN dotnet restore "ProductCatalog.Api/ProductCatalog.Api.csproj"
COPY . .
WORKDIR "/src/ProductCatalog.Api"
RUN dotnet build "ProductCatalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductCatalog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
# This root cert file is copied from the original cert location so it can be included in the image
# This is required since the product catalog api calls the identity server and root verification will fail without it.
ADD myCA.pem /usr/local/share/ca-certificates/myCA.crt
RUN update-ca-certificates
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductCatalog.Api.dll"]