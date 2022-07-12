FROM microsoft/dotnet:2.2-sdk-nanoserver-sac2016 AS base

WORKDIR /app

WORKDIR /Evolution.Services

COPY . .

WORKDIR /Evolution.Services/Evolution.Document.DeleteOrphanDocument

RUN dotnet restore -nowarn:msb3202,nu1503

RUN dotnet build --no-restore -c Release -o /app

FROM base AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Evolution.Document.DeleteOrphanDocument.dll"]