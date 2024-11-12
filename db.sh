#!/usr/bin/env bash

docker run --name knive-db \
    -e POSTGRES_PASSWORD=password \
    -e POSTGRES_USER='knive' \
    -e POSTGRES_DB='knive' \
    -p 5432:5432 \
    -d postgres
