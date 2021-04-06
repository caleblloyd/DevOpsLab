#!/bin/sh -e

cd "$(dirname "$0")"

docker exec -it \
    dol-dotnet \
    dotnet tool restore
    
docker exec -it \
    -w "/dotnet/src/Server" \
    dol-dotnet \
        dotnet outdated \
        -vl "major" \
        "$@"
