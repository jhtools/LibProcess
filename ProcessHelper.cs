using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LibProcess;

public class ProcessHelper : IProcessHelper
{
    private readonly ILogger<ProcessHelper>? _logger;

    public ProcessHelper(ILogger<ProcessHelper>? logger = null)
    {
        _logger = logger;
    }

    public async Task<int> StartProcessAsync(
        string fileName,
        ProcessArgumentHandler? arguments = null,
        string? workingDirectory = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken? cancellationToken = null)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = outputDataReceived != null || _logger != null,
                RedirectStandardError = errorDataReceived != null || _logger != null,
            },
            EnableRaisingEvents = true
        };
        var strArguments = arguments?.FillStartupInfo(process.StartInfo);
        _logger?.LogDebug("Starting process {FileName} {Arguments} in working directory {WorkingDirectory}",
            fileName, strArguments, workingDirectory);
        var semaphoreOutput = new SemaphoreSlim(0, 1);
        if (outputDataReceived != null || _logger != null)
        {
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    outputDataReceived?.Invoke(args.Data);
                    _logger?.LogDebug("Process {FileName} output: {Data}", fileName, args.Data);
                }
                else
                {
                    semaphoreOutput.Release();
                    _logger?.LogDebug("Process {FileName} output closed", fileName);
                }
            };
        }

        var semaphoreError = new SemaphoreSlim(0, 1);
        if (errorDataReceived != null || _logger != null)
        {
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null)
                {
                    errorDataReceived?.Invoke(args.Data);
                    _logger?.LogDebug("Process {FileName} error: {Data}", fileName, args.Data);
                }
                else
                {
                    semaphoreError.Release();
                    _logger?.LogDebug("Process {FileName} error closed", fileName);
                }
            };
        }

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        var ct = cancellationToken ?? CancellationToken.None;
        _logger?.LogDebug("Waiting for process {FileName} to exit", fileName);
        await process.WaitForExitAsync(ct);
        await semaphoreError.WaitAsync(ct);
        await semaphoreOutput.WaitAsync(ct);
        _logger?.LogDebug("Process {FileName} exited with code {ExitCode}", fileName, process.ExitCode);
        return process.ExitCode;
    }
}