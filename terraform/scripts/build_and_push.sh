#!/bin/bash
set -e

eval $(minikube -p minikube docker-env)
docker build -t localhost:5000/store-api:latest ../application/backend
docker push localhost:5000/store-api:latest