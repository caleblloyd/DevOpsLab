#!/bin/sh -e

cd $(dirname $0)

docker exec -i \
    -e "ASPNETCORE_ENVIRONMENT=DockerCompose" \
    -w "/dotnet/src/Server" \
    dol-dotnet \
    dotnet run --configuration "EF" dropDb
    
docker exec -i \
    -e "ASPNETCORE_ENVIRONMENT=DockerCompose" \
    -w "/dotnet/src/Server" \
    dol-dotnet \
    dotnet run --configuration "EF" migrate

touch src/Server/DevOpsLab.Server.csproj
