namespace LibProcess;

public interface IProcessHelper
{
    Task<int> StartProcessAsync(
        string fileName,
        ProcessArgumentHandler? arguments = null,
        string? workingDirectory = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken? cancellationToken = null);
}