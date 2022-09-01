# syntax=docker/dockerfile:1

####################################
# Cratis Server
# Build runtime image
####################################
FROM mcr.microsoft.com/dotnet/runtime:6.0.1-focal
ARG CONFIGURATION=Release
ARG VERSION

WORKDIR /app

RUN echo Configuration = ${CONFIGURATION}
ENV PATH="$PATH:/root/.pulumi/bin"

# Install Tools
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    nano \
    iputils-ping \
    netcat \
    wget \
    curl \
    gnupg \
    apt-transport-https \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

# Install Pulumi
RUN curl -fsSL https://get.pulumi.com | sh
RUN echo "export PATH=$PATH:/root/.pulumi/bin" >> /root/.profile

COPY Docker/copy-server-files.sh ./copy-server-files.sh
RUN chmod +x ./copy-server-files.sh

COPY ./Source/Main/out ./out
COPY ./Source/Web/wwwroot wwwroot

RUN echo $PWD
RUN ./copy-server-files.sh

# Expose the Web and Orleans ports
EXPOSE 80 11111 30000

ENTRYPOINT ["./Main"]
