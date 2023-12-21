# To Do Rest API

## Stack

- [X]   Docker
- [X]   .NET 8 
- [X]   Entity Framework Core
- [X]   PostgreSQL
#### To implement
- [ ]   JWT
- [ ]   Redis
- [ ]   RabbitMQ

#### Command to run
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
```
 dotnet tool install --global dotnet-ef
```
### Add migrations
```
 dotnet ef migrations add "migration-name"
```
### Create database
```
 dotnet ef database update
```