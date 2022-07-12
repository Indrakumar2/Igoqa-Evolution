# escape=`
FROM evolution/node_modules:R3.0.1 as evo2node_modules

FROM windows-node as base

WORKDIR "C:/app"

COPY --from=evo2node_modules "C:/app" "C:/app"

COPY . "C:/app"
  
ENV REACT_APP_API_BASE_URL=https://10.1.1.100:8080/ 
ENV REACT_APP_UPLOAD_FILE_LIMIT=204800
ENV REACT_APP_IDLE_TIMEOUT_IN_SECONDS=3600
ENV REACT_APP_IDLE_WAITING_TIME_IN_MILLISECONDS=60000 
ENV REACT_APP_EXTRANET_URL=https://evolution.moodyint.com/DLive/extranet/Client/Visit.aspx?Id=
ENV REACT_APP_APPLICATION_VERSION_NUMBER=3.0.D.017
RUN npm run build 

FROM mcr.microsoft.com/windows/servercore/iis:windowsservercore-ltsc2016

# Install IISRewrite Module 
ADD https://download.microsoft.com/download/1/2/8/128E2E22-C1B9-44A4-BE2A-5859ED1D4592/rewrite_amd64_en-US.msi c:/inetpub/rewrite_amd64_en-US.msi
RUN powershell -Command Start-Process c:/inetpub/rewrite_amd64_en-US.msi -ArgumentList "/qn" -Wait

WORKDIR /inetpub/wwwroot
EXPOSE 80 443 
COPY --from=base  C:\app\build\ .
COPY SSL.pfx .
COPY certificate.ps1 .
COPY web.config .
RUN powershell.exe ./certificate.ps1



 