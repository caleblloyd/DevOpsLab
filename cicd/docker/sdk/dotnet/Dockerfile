FROM alpine:latest AS code
COPY . /dotnet
RUN mkdir /csproj \
    && cd /dotnet \
    && find . -type f \
        \( -iname "*.csproj" -o -iname "*.sln" -o -iname "nuget.config" \) \
        -exec cp --parents {} /csproj \;

FROM mcr.microsoft.com/dotnet/sdk:5.0.103

# add docker user and setup folders
RUN addgroup --gid 1000 docker && \
    adduser --uid 1000 --ingroup docker --home /home/docker --shell /bin/sh --disabled-password --gecos "" docker

# set user
USER docker:docker

# restore
COPY --from=code --chown=docker:docker /csproj /dotnet
RUN cd /dotnet \
    && dotnet restore

# copy source code
COPY --chown=docker:docker src/ /dotnet/src/
RUN cd /dotnet \
    && dotnet build -c Debug

# set workdir and command
WORKDIR /dotnet/src/DevOpsLab
CMD ["dotnet", "run", "-c", "Debug"]
