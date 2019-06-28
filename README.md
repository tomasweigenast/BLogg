# BLogg - Making Logging Easier
BLogg provides you a fast and easy way to log events in your applications.

> It's cross-platform so you can use it in Xamarin, MVC, .NET Core, Mono and .NET Framework projects

[![Build Status](https://img.shields.io/travis/com/Tomi-15/BLogg.svg?style=for-the-badge)](https://travis-ci.com/Tomi-15/BLogg)
[![License](https://img.shields.io/badge/license-GNU%20GPLv3-blue.svg?style=for-the-badge)](https://github.com/Tomi-15/BLogg/blob/master/LICENSE.txt)
[![OpenIssues](https://img.shields.io/github/issues-raw/Tomi-15/BLogg.svg?style=for-the-badge)](https://github.com/Tomi-15/BLogg/issues)
![Discord](https://img.shields.io/badge/Discord-Tomas%238453-orange.svg?style=for-the-badge&logo=discord)


## Features
- Familiar log levels *(Debug, Info, Warning, Error and Fatal)*
- Fast implementation on any platform
- Dependency injection compatible
- Processors to save or show log events
- Custom log text format for each processor

## Installation
You can install the latest version of BLogg via **Nuget Package Manager**

``` Shell
PM> Install-Package BLogg.Core
```

## Usage
To start logging, you need a `Logger` instance.

### **Creating a new logger**
---

To create a new logger instance, the `MakeNew` static method, found in the `LoggerMaker` class, is used. It doesn't take any parameter but it returns an instance of the `LoggerMaker` class which is used to configure the logger.

``` csharp
LoggerMaker maker = LoggerMaker.MakeNew();
```

### **Configuring the logger**
---

To configure the logger, you have different methods and properties to make it as you want:

#### Processors
You can specify where the event will go when its logged by adding different processors. There are two built-in processors: 

- `Console(ConsoleProcessorSettings)` *It will process the events and show them in a console*
- `File(FileProcessorSettings)` *It will process the events saving them to files*

> You can create your own processor by implemeting the `ILogProcessor` interface. [See the Wiki](https://github.com/Tomi-15/BLogg/wiki)

**Example:**
```csharp
maker.WithProcessor.Console(settings => 
{
    settings.UseColors = true; // Will print the events in colors
    settings.CustomOutputFormat = "[Timestamp] [Level] [Message]"; // Custom log output format
})
.WithProcessor.File(settings => 
{
    settings.Path = "App\\logs"; // Required property. Specifies the directory to save log files.
    settings.FileSizeLimit = 1000 * 1000; // File size limit in bytes
});
```
#### Levels
Also, you can specify a default log level to use:

```csharp
maker.WithDefaultLogLevel(LogLevel.Debug);
```

### **Building the logger**
---
After configuring the logger, the `Build(bool)` is used to create the logger. It will configure it for you, and returns an instance. It takes one optional parameter:

 `addToGlobal` **bool**
>By default is true and indicates if the logger to create must be added to the global static variable.

```csharp
Logger logger = maker.Build();
```

### **Using the logger**
---
To log events you can use:

**Log a simple message using the default Level**
```csharp
logger.Log("Hello world!");
```

**Log a simple message with a custom log level**
```csharp
logger.Log("A fatal error!", LogLevel.Fatal);
```

**Another ways of logging**
```csharp
logger.LogDebug("Action finished. It took 54 ms."); // Log debug information
// OUTPUT: [Debug] Action finished. It took 54 ms.

logger.LogInformation("The file {Filename} was downloaded.", fileName); // You can format the message
// OUTPUT: [Info] The file info.txt was downloaded.

logger.LogWarning("Resources could not be downloaded. Response: {@ServerResponse}", response); // You can format the message adding a Json formatting to the object
// OUTPUT: [Warning] Resources could not be donwloaded. Response: { "description:" "Not found", "code": 404 }
}

logger.LogError("Cannot divide by 0.", ex); // Also you can log an exception
// OUTPUT: [Error] Cannot divide by 0. 
//                 System.DivideByZeroException: Attempted to divide by zero.

logger.LogFatal("Run script cannot be found and the application cannot start.");
// OUTPUT: [Fatal] Run script cannot be found and the application cannot start.
```

## License
Licensed under **GNU General Public License v3.0**

>For more information read LICENSE.txt

## Donate
[Buy me](https://www.paypal.me/tomasweg15?locale.x=es_XC) a Fernet ;)
