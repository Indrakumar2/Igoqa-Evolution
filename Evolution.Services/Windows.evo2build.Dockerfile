FROM microsoft/dotnet:2.2-sdk-nanoserver-sac2016 AS evo2_build

WORKDIR /Evolution2/Evolution.Services

COPY . /Evolution2/Evolution.Services

RUN  ./Windows.Compile.All.Services.bat
