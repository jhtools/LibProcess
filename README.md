# LibProcess

## Introduction

LibProcess is a library for starting processes in C#.

**WARNING**: This library is still in development and is not yet ready for production use.

## License

LibProcess is licensed under the MIT license. See the LICENSE file for more information.


## Installation

LibProcess is available as a NuGet package. To install LibProcess, run the following command in the Package Manager Console:

    PM> Install-Package jhtools.LibProcess

## Dotnet Support

LibProcess is compatible with .NET 6.0 and later.

## Arguments

In general, arguments can be passed to a process in two ways:

1. As a string enumerable
2. As a string

If you pass a string enumerable, the arguments will be escaped and joined with spaces. If you pass a string, the arguments will not be escaped or joined.

In particular, if you pass a string, you are responsible for escaping and joining the arguments yourself.
In many cases that may be more convenient, but it is also more error-prone.

## Logging

The `ProcessHelper` class has a parameter `logger` that can be used to log the output of the process and some additional information.
You can pass a null if you don't want to log anything.

## ProcessHelper

The `ProcessHelper` class is the main class of the library. It provides methods for starting processes and waiting for them to exit.

### StartProcessAsync

```csharp
async Task<int> StartProcessAsync(
        string fileName,
        IEnumerable<string> arguments = null,
        string? workingDirectory = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken? cancellationToken = null)
```

Starts a process and waits for it to exit. The process is started asynchronously. The method returns the exit code of the process.

The `workingDirectory` parameter can be used to specify the working directory of the process. If it is `null`, the working directory of the current process is used.

The `outputDataReceived` and `errorDataReceived` parameters can be used to specify callbacks that are called when the process writes to `stdout` or `stderr`.

There is an overload of this method that takes a `string` instead of an `IEnumerable<string>` for the `arguments` parameter. In that case, the arguments are not escaped or joined.

There is also an overload with this signature:

```csharp
Task<int> StartProcessAsync(
        this IProcessHelper processHelper,
        CancellationToken ct,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null)
```

This overload is useful if you want to use the logging functionality of the `ProcessHelper` class but need a `CancellationToken`.

### StartProcessWithResultAsync

```csharp
    public static async Task<(int exitCode, string output, string error)> StartProcessWithResultAsync(
        string fileName,
        IEnumerable<string> arguments,
        string? workingDirectory = null,
        CancellationToken? ct = null)
```

Starts a process and waits for it to exit. The process is started asynchronously. The method returns the exit code of the process, the output of the process and the error output of the process.

There are also overloads of this method that take a `string` instead of an `IEnumerable<string>` for the `arguments` parameter. In that case, the arguments are not escaped or joined.

There is also an overload with this signature:

```csharp
Task<(int exitCode, string output, string error)> StartProcessWithResultAsync(
        CancellationToken ct,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null)
```
This overload is useful if you want to use the logging functionality of the `ProcessHelper` class but need a `CancellationToken`.

## Unit Test Support

The functionality is provided via the interface `IProcessHelper`. This interface is implemented by the class `ProcessHelper`.

This allows you to write unit tests that use the `ProcessHelper` class by creating a mock of the `IProcessHelper` interface.