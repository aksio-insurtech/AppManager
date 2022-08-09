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
RUN echo Version = ${VERSION}

COPY Docker/copy-server-files.sh ./copy-server-files.sh
RUN chmod +x ./copy-server-files.sh

COPY ./Source/Main/out ./out
COPY ./Source/Web/wwwroot wwwroot

RUN echo $PWD
RUN ./copy-server-files.sh

EXPOSE 80 11111 30000

ENTRYPOINT ["./Main"]
