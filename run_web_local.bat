@echo off
echo  Welcome to HealthyJuices.Web

cd HealthyJuices.Web

IF EXIST node_modules (
echo Running app
) ELSE (
echo Installing packages
call npm install -g @angular/cli
call npm i
)

call ng serve -o 

pause