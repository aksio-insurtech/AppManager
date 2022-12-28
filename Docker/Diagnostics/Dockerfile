# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    dnsutils \
    traceroute \
    iputils-ping \
    net-tools \
    nano \
    netcat \
    wget \
    curl \
    gnupg \
    apt-transport-https \
    ca-certificates \
    openssl \
    && rm -rf /var/lib/apt/lists/*

# Install MongoDB
RUN wget --no-check-certificate -qO - https://www.mongodb.org/static/pgp/server-5.0.asc | apt-key add - \
    && echo "deb [ arch=$(dpkg --print-architecture) ] https://repo.mongodb.org/apt/ubuntu bionic/mongodb-org/5.0 multiverse" > /etc/apt/sources.list.d/mongodb-org-5.0.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
    mongodb-org-server \
    mongodb-org-shell \
    mongodb-database-tools \
    mongodb-atlas \
    && rm -rf /var/lib/apt/lists/*

CMD sleep infinity
