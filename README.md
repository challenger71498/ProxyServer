# ProxyServer

A proxy server for MySQL.

# Prerequisites

## MySQL Docker Container

Image (latest): https://hub.docker.com/_/mysql

### Configuration

```bash
docker run --rm --network host --name mysql -e MYSQL_ROOT_PASSWORD=foobar mysql:latest --authentication-policy="*,,"
```

# Start

## Server

```bash
dotnet run --project MySqlProxyServer
```

## Client

### Mysql Console

```bash
mysql --ssl-mode=DISABLED --default-auth=mysql_native_password --host=127.0.0.1 --port=8080 --user=<any username> --password=<any password>
```

# Features

## Login & Query

![image](https://user-images.githubusercontent.com/43464986/146758272-faccf172-6ae4-4f69-914a-8c26d19983f4.png)

### Login with any account

> Only works if the auth method is `mysql_native_password`.

![image](https://user-images.githubusercontent.com/43464986/146758765-888a1736-936b-4138-be2d-75045b10accf.png)

## Multiconnections

![image](https://user-images.githubusercontent.com/43464986/146758926-89b603cb-3d0a-4e10-a050-56dadc3d09fa.png)

## Reject Query if contains `CHEQUER`

![image](https://user-images.githubusercontent.com/43464986/146759093-17d20ccb-9d25-40c3-b232-61daf2072764.png)

## Logging

Log files are located at `MySqlProxyServer/bin/Debug/net5.0/logs`.

Format of the log file name is: `YY-MM-DD`.

![image](https://user-images.githubusercontent.com/43464986/146758517-1332249c-496b-4f32-ba97-3f8996fb3a83.png)

## Random Masking

Simply put `RANDOM_MASK` at the end of your query.

```sql
SELECT user from mysql.user RANDOM_MASK;
```

![image](https://user-images.githubusercontent.com/43464986/146757970-594ffce7-937d-4ac1-9ac5-c1b832e5de35.png)

# Limitations

-   Currently only `mysql_native_password` authentication is supported.

-   Graceful exit on socket is not implemented.

# Known Issues

- [x] ~~Socket is not released when socket connection finishes.~~
