# Exception Handler

Exception Handler is a C# library to handle automatically .NET 8 Web Api's exceptions.

## Installation

Use the package manager.

## Usage
Just with one line of code:

```C#
builder.ConfigureSerilogAndExceptionHandler("LogConfiguration");
```

## Requirements

    - Minimum .NET 8 version (https://dotnet.microsoft.com/es-es/download/dotnet/8.0).

    - The 'logSection' param when calling
      ConfigureSerilogAndExceptionHandler() can be named as you wish, but its content
      must have at least one level and both keys 'Level' and 'Path'. If those keys are
      not supplied an error will be thrown.

## Example of appsettings.json entry.

    "LogConfiguration": 
    [
        {
            "Level": "Debug",
            "Path": "./logs/debug-logs-.json"
        },
        {
          "Level": "Information",
          "Path": "./logs/information-logs-.json"
        },
        {
          "Level": "Warning",
          "Path": "./logs/warning-logs-.json"
        },
        {
          "Level": "Error",
          "Path": "./logs/error-logs-.json"
        }
    ]

## Serilog

    - Levels: It's important to configure existing
    levels within Serilog LogEventLevel, which can be found
    at: https://github.com/serilog/serilog/wiki/Configuration-Basics.
    If a non existing level is introduced, an error will be thrown.

    - No Serilog installation package needed from the Api, everything is managed from 
    the Exception Handler.



## License

[MIT](https://choosealicense.com/licenses/mit/)