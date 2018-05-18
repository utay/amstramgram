#!/bin/bash

# wait for sqlserver to start
sleep 10

# run migrations
(
    cd /app/Data
    dotnet ef database update
    dotnet sql-cache create 'Server=sqlserver;Database=master;User=sa;Password=Strong(!)Password;' "dbo" "Session"
)

# start the application
cd /app/Api
dotnet ./bin/Release/netcoreapp2.0/Api.dll
