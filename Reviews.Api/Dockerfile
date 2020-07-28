#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Reviews.Api/Reviews.Api.csproj", "Reviews.Api/"]
COPY ["Reviews.Models/Reviews.Models.csproj", "Reviews.Models/"]
COPY ["Reviews.Repository/Reviews.Repository.csproj", "Reviews.Repository/"]
COPY ["Reviews.Shared/Reviews.Shared.csproj", "Reviews.Shared/"]
RUN dotnet restore "Reviews.Api/Reviews.Api.csproj"
COPY . .
WORKDIR "/src/Reviews.Api"
RUN dotnet build "Reviews.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reviews.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
# This root cert file is copied from the original cert location so it can be included in the image
# This is required since the reviews api calls the identity server and root verification will fail without it.
ADD myCA.pem /usr/local/share/ca-certificates/myCA.crt
RUN update-ca-certificates
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reviews.Api.dll"]