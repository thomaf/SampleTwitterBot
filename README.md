# SampleTwitterBot
Little tweet genrator using dotnet core

## Installing dotnet core
Follow instructions here : https://www.microsoft.com/net/core

## Create configuration file
You need a file `config.json` in the root folder with the authentication informations like this :
```json
{
    "twitterAuth": {
        "consumerKey": "1234567890123456789012345",
        "consumerSecret": "12345678901234567890123456789012345678901234567890",
        "userAccessToken": "123456789012345678-1234567890123456789012345678901",
        "userAccessSecret": "123456789012345678901234567890123456789012345"
    }
}
```

## Retrieve dependencies
```sh
dotnet restore
```

## Building project
```sh
dotnet build
```

## Then run
```sh
dotnet run
```
