#!/bin/bash

# wait for sqlserver to start
sleep 10

# run migrations
(
    cd /app/Data
    dotnet ef database update
)

# start the application
cd /app/Api
dotnet ./bin/Release/netcoreapp2.0/Api.dll
