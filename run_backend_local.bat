@echo off
echo Welcome to HealthyJuices.Backend

cd HealthyJuices.Backend
call dotnet restore
call dotnet build 

cd 1Presentation/HealthyJuices.Api

start dotnet run --launch-profile "Local"

start "" http://localhost:5000/swagger