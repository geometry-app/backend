#!/bin/bash

app=$1
image=$2
tag=$3

rm -rf out/*

cat Dockerfile | sed -r "s/AppEntryPoint/$app/" > TemporaryDockerfile
dotnet publish -c Release $app/$app.csproj -o out
git rev-parse HEAD > out/version.txt
docker build -t $image -f TemporaryDockerfile --no-cache .
docker tag $image $GEOMETRYAPP_DEPLOYMENT_REGISTRY/geometryapp/$image:$tag
docker push $GEOMETRYAPP_DEPLOYMENT_REGISTRY/geometryapp/$image:$tag
docker tag $image $GEOMETRYAPP_DEPLOYMENT_REGISTRY/geometryapp/$image:latest
docker push $GEOMETRYAPP_DEPLOYMENT_REGISTRY/geometryapp/$image:latest
