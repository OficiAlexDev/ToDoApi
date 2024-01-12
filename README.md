<p align="center">
  <a href="https://oficialexdev.github.io/portifolio/#/" target="_blank"><img src="./a.svg" width="200" alt="Alex Logo" /></a>
</p>

# To Do
#### Command to run app
```
docker-compose up --build
``` 
## Stack
### Back-End Rest API
- [X]   Docker
- [X]   .NET 8 
- [X]   Entity Framework Core
- [X]   PostgreSQL
- [X]   JWT
- [X]   MS Test
- [X]   Redis
#### To implement
- [ ]   JWT Refresh Token
- [ ]   Email confirm create user
### Front-End
- [X]   Angular
- [X]   Tailwind
- [X]   Figma Layout
 
#### Figma
Logo
<p align="center">
  <a href="https://oficialexdev.github.io/portifolio/#/" target="_blank"><img src="./logo.svg" width="256" alt="ToDo Logo" /></a>
</p>


#### Colors docs

| Color               | Hex                                               |
| ----------------- | ---------------------------------------------------------------- |
| Primary      | ![#1342FA](https://via.placeholder.com/10/1342FA?text=+) #1342FA |
| Primary Light      | ![#206BFF](https://via.placeholder.com/10/206BFF?text=+) #206BFF |
| Primary Dark     | ![#061860](https://via.placeholder.com/10/061860?text=+) #061860 |


#### Host to acess web
``` 
http://localhost:4201/
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
Have sure you have .NET SDK, Postgres and Redis installed to run this app, and change the env variables on appSettings on backend project.
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