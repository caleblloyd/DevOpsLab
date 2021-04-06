ARG GIT_COMMIT_SHORT_HASH
FROM dol-sdk:${GIT_COMMIT_SHORT_HASH} AS sdk

RUN cd /dotnet/src/DevOpsLab \
    && dotnet publish -o app

FROM mcr.microsoft.com/dotnet/aspnet:5.0.3

# add docker user, group, and setup directories
RUN addgroup --gid 1000 docker && \
    adduser --uid 1000 --ingroup docker --home /home/docker --shell /bin/sh --disabled-password --gecos "" docker

# set user
USER docker:docker

# copy application
COPY --from=sdk --chown=docker:docker /dotnet/src/DevOpsLab/app /app

# set workdir and command
WORKDIR /app
CMD ["./DevOpsLab"]
