using System.Text;

namespace LibProcess;

public static class ProcessHelperExtensions
{
    public static async Task<int> StartProcessAsync(
        this IProcessHelper processHelper,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken? ct = null)
    {
        return await processHelper.StartProcessAsync(
            fileName,
            arguments == null ? null : new ProcessArgumentHandler(arguments),
            workingDirectory,
            outputDataReceived,
            errorDataReceived,
            ct);
    }
    
    public static async Task<int> StartProcessAsync(
        this IProcessHelper processHelper,
        string fileName,
        IEnumerable<string> arguments,
        string? workingDirectory = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken? ct = null)
    {
        return await processHelper.StartProcessAsync(
            fileName,
            new ProcessArgumentHandler(arguments),
            workingDirectory,
            outputDataReceived,
            errorDataReceived,
            ct);
    }
    
    public static async Task<int> StartProcessAsync(
        this IProcessHelper processHelper,
        CancellationToken ct,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null)
    {
        return await processHelper.StartProcessAsync(
            fileName,
            arguments == null ? null : new ProcessArgumentHandler(arguments),
            workingDirectory,
            null,
            null,
            ct);
    }
    
    public static async Task<int> StartProcessAsync(
        this IProcessHelper processHelper,
        CancellationToken ct,
        string fileName,
        IEnumerable<string>? arguments = null,
        string? workingDirectory = null)
    {
        return await processHelper.StartProcessAsync(
            fileName,
            arguments == null ? null : new ProcessArgumentHandler(arguments),
            workingDirectory,
            null,
            null,
            ct);
    }
    
    public static async Task<(int exitCode, string output, string error)> StartProcessWithResultAsync(
        this IProcessHelper processHelper,
        string fileName,
        string? arguments = null,
        string? workingDirectory = null,
        CancellationToken? ct = null)
    {
        return await InternalStartProcessWithResultAsync(
            processHelper,
            fileName,
            arguments == null ? null : new ProcessArgumentHandler(arguments),
            workingDirectory,
            ct);
    }

    public static async Task<(int exitCode, string output, string error)> StartProcessWithResultAsync(
        this IProcessHelper processHelper,
        string fileName,
        IEnumerable<string> arguments,
        string? workingDirectory = null,
        CancellationToken? ct = null)
    {
        return await InternalStartProcessWithResultAsync(
            processHelper,
            fileName,
            new ProcessArgumentHandler(arguments),
            workingDirectory,
            ct);
    }

    
    
    private static async Task<(int exitCode, string output, string error)> InternalStartProcessWithResultAsync(
        IProcessHelper processHelper,
        string fileName,
        ProcessArgumentHandler? arguments = null,
        string? workingDirectory = null,
        CancellationToken? ct = null)
    {
        var sbOut = new StringBuilder();
        var sbErr = new StringBuilder();
        var rc = await processHelper.StartProcessAsync(
            fileName,
            arguments,
            workingDirectory,
            s => sbOut.AppendLine(s),
            s => sbErr.AppendLine(s), ct);

        return (rc, sbOut.ToString(), sbErr.ToString());
    }
}