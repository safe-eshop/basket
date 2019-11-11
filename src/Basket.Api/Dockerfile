FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100 AS build
WORKDIR /src
COPY . .
RUN dotnet tool install --global paket
ENV PATH="/root/.dotnet/tools:${PATH}"

WORKDIR /src/src/Basket.Api
RUN paket restore
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build -c Release -o /app

FROM build as test
WORKDIR /src/tests/Basket.DomainUnitTests
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet test

FROM build as test
WORKDIR /src/tests/Basket.ApplicationUnitTests
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet test

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]