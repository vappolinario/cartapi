FROM bitnami/mariadb:latest
ENV MARIADB_ROOT_PASSWORD=admin
COPY ./sql/*.sql /docker-entrypoint-initdb.d/
