@echo off
set environment=%1
if [%1] == [] (
 echo No argument supplied, Please pass environment value specified in Docker override file
 goto break 
) 

FOR /F "tokens=* USEBACKQ" %%F IN (`docker images -q evolution/evo2build:R3.0.1`) DO (
SET imgEvo2build=%%F
)  
if [%imgEvo2build%] == [] (
   echo Building evolution/evo2build:R3.0.1 image
   docker build  -t evolution/evo2build:R3.0.1 -f Windows.evo2build.Dockerfile .
)

docker-compose -f %environment%.Windows.docker-compose.yml -f %environment%.Windows.docker-compose.override.yml --compatibility  up -d --no-recreate --scale %environment%.evolution.api=3 --scale %environment%.evolution.admin.api=5
docker-compose -f %environment%.Windows.MongoSync.docker-compose.yml -f %environment%.Windows.MongoSync.docker-compose.override.yml run -d %environment%.evolution.document.mongosync Failed
docker-compose -f %environment%.Windows.MongoSync.docker-compose.yml -f %environment%.Windows.MongoSync.docker-compose.override.yml run -d %environment%.evolution.document.mongosync New
 
 :break