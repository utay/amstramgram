# Amstramgram

## Install

Install nuget packages and build GraphiQL

`cd Api && npm install && npm run start`

`cd Api && dotnet restore`

`cd Core && dotnet restore`

`cd Data && dotnet restore`

## Migrate

Run SQL Server with docker

```
docker run
    -e 'ACCEPT_EULA=Y'
    -e 'SA_PASSWORD=Strong(!)Password'
    -p 1433:1433
    -d microsoft/mssql-server-linux:2017-latest
```

Migrate

`cd Data && dotnet ef database update`

Session store

`cd Api && dotnet sql-cache create 'Server=localhost;Database=master;User=sa;Password=Strong(!)Password;' "dbo" "Session"`

## Run

Start the API

`cd Api && dotnet run`
