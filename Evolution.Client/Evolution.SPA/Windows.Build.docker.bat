@echo off

FOR /F "tokens=* USEBACKQ" %%F IN (`docker images -q evolution/node_modules:R3.0.1`) DO (
SET imgEvo2build=%%F
)  
if [%imgEvo2build%] == [] (
   echo Building evolution/node_modules:R3.0.1 image
   docker build  -t evolution/node_modules:R3.0.1 -f Windows.Evolution.SPA.Node_modules.Dockerfile .
)

docker-compose -f Windows.docker-compose.yml -f Windows.docker-compose.override.yml build --compress
docker-compose -f Windows.docker-compose.yml -f Windows.docker-compose.override.yml --compatibility  up -d 
