FROM mcr.microsoft.com/dotnet/sdk:5.0.103

# add docker user and setup folders
RUN addgroup --gid 1000 docker && \
    adduser --uid 1000 --ingroup docker --home /home/docker --shell /bin/sh --disabled-password --gecos "" docker && \
    mkdir -p /dotnet && \
    chown -R docker:docker /dotnet

# install fixuid
RUN curl -SsL https://github.com/boxboat/fixuid/releases/download/v0.4/fixuid-0.4-linux-amd64.tar.gz | tar -C /usr/local/bin -xzf - && \
    chown root:root /usr/local/bin/fixuid && \
    chmod 4755 /usr/local/bin/fixuid

# create directories
RUN mkdir -p /dotnet/.bin && \
    mkdir -p /dotnet/.obj && \
    chown -R docker:docker /dotnet && \
    mkdir -p /home/docker/.nuget && \
    chown docker:docker /home/docker/.nuget

# set workdir, entrypoint, and command
WORKDIR /dotnet
ENTRYPOINT ["fixuid"]
CMD ["dotnet", "watch", "-p", "src/Server", "run", "-c", "Debug"]

# copy staged files
COPY stage/ /

# set user
USER docker:docker
