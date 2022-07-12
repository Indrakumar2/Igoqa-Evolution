FROM evolution/evo2build:R3.0.1 as evo2build

FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-sac2016 AS base

WORKDIR /app

COPY --from=evo2build /Evolution2/publish/Evolution.Api /app

EXPOSE 80

ENTRYPOINT ["dotnet", "Evolution.Api.dll"]
