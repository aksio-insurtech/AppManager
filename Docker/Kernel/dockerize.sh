#!/bin/bash
docker buildx build -t opensjoncontainerapp4716a5a2.azurecr.io/cratis:6.15.6 -f ./Dockerfile --platform linux/amd64 --push ../
