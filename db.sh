#!/usr/bin/env bash

docker run --name kitchen-helper-postgres \
    -e POSTGRES_PASSWORD=password \
    -e POSTGRES_USER='kitchen-helper' \
    -e POSTGRES_DB='kitchen-helper' \
    -p 5432:5432 \
    -d postgres
