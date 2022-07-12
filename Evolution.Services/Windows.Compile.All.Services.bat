@echo off 
setlocal enabledelayedexpansion
set CUR_DIR=%cd%
set tm_str=%date:~10,4%-%date:~4,2%-%date:~7,2%
set BUILD_DIR=../../build
set PUBLISH_DIR=../../publish
set LOG_FILE=../../build/Evo2BuildLog_%tm_str%.log

set ALL_DIR[0]=Evolution.Api
set ALL_DIR[1]=Evolution.Admin.Api
set ALL_DIR[2]=Evolution.Document.MongoSync
set ALL_DIR[3]=Evolution.Document.DeleteOrphanDocument
set ALL_DIR[4]=Evolution.Email.ExpiryNotification
set ALL_DIR[5]=Evolution.Email.Notification
set ALL_DIR[6]=Evolution.Web.Gateway
set ALL_DIR[7]=Evolution.AuthorizationService

for /l %%n in (0,1,7) do ( 
   echo !ALL_DIR[%%n]!   
   cd %CUR_DIR%/!ALL_DIR[%%n]!/
   dotnet restore /flp:v=diag /flp:logfile=%LOG_FILE% -nowarn:msb3202,nu1503
   dotnet build /flp:v=diag /flp:logfile=%LOG_FILE% --no-restore -c Release -o %BUILD_DIR%/!ALL_DIR[%%n]!
   dotnet publish /flp:v=diag /flp:logfile=%LOG_FILE% --no-restore -c Release -o %PUBLISH_DIR%/!ALL_DIR[%%n]!
)

echo Compilation of all sources completed. Log file is stored in %LOG_FILE%

cd %CUR_DIR%