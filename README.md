A proxy server for MySQL.

# Prerequisites

## MySQL Docker Container

Image (latest): https://hub.docker.com/_/mysql

### Configuration

```
docker run --rm --network host --name mysql -e MYSQL_ROOT_PASSWORD=foobar mysql:latest --authentication-policy="*,,"
```

# Start

## Server

```
dotnet run --project MySqlProxyServer
```

## Client

### Mysql Console

```
mysql --ssl-mode=DISABLED --default-auth=mysql_native_password --host=127.0.0.1 --port=8080 --user=<any username> --password=<any password>
```

## Logging

Log files are located at `MySqlProxyServer/bin/Debug/net5.0/logs`.

# Limitations

- Currently only `mysql_native_password` authentication is supported.

- Graceful exit on socket is not implemented.

# Known Issues

- [ ] Critical memory leak issue when socket connection finishes.
