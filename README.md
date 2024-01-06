<p align="center">
  <a href="https://oficialexdev.github.io/portifolio/#/" target="_blank"><img src="./a.svg" width="200" alt="Alex Logo" /></a>
</p>

# To Do 
## Stack
### Back-End Rest API
- [X]   Docker
- [X]   .NET 8 
- [X]   Entity Framework Core
- [X]   PostgreSQL
- [X]   JWT
- [X]   MS Test
#### To implement
- [ ]   JWT Refresh Token
- [ ]   Email confirm create user
- [ ]   Redis
### Front-End
#### To implement
- [ ]   Angular
- [ ]   Tailwind

## API
#### Command to run backend
```
docker-compose up --build
``` 
#### Host to acess API
``` 
https://localhost:8081/
``` 

## API DOCS

Use Swagger to see API methods
``` 
https://localhost:8081/swagger
``` 
 
## Run without docker
Have sure you have .NET SDK and Postgres installed to run this commands 
 ### Check if EF tool are install
```
 dotnet-ef
```
### Install EF tool 
#### Be sure you are inside project folder, not only in solution folder!
```
 dotnet tool install --global dotnet-ef
```
### Add migrations
```
 dotnet ef migrations add "ToDoMigration"
```
### Create database
```
 dotnet ef database update
```