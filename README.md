# Prerequisites

## MySQL Docker Container

Image (latest): https://hub.docker.com/_/mysql

### Configuration

```
docker run --rm --network host --name mysql -e MYSQL_ROOT_PASSWORD=foobar mysql:latest --authentication-policy="*,,"
```

# Start

```
dotnet run --project MySqlProxyServer
```

## Logging

Log files are located at `MySqlProxyServer/bin/Debug/net5.0/logs`.

# Limitations

- Currently only `mysql_native_password` authentication is supported.

- Graceful exit on socket is not implemented.

# Known Issues

- [ ] Critical memory leak issue when socket connection finishes.
