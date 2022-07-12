FROM evolution/evo2build:R3.0.1 as evo2build
FROM microsoft/dotnet:2.2-aspnetcore-runtime-nanoserver-sac2016 AS base

RUN  mkdir -p  /https
COPY SSL.pfx /https
WORKDIR /app

COPY --from=evo2build /Evolution2/publish/Evolution.Web.Gateway /app

EXPOSE 80 443

ENTRYPOINT ["dotnet", "Evolution.Web.Gateway.dll"]
