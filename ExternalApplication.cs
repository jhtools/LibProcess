namespace LibProcess;

public class ExternalApplication
{
    private readonly IProcessHelper _processHelper;
    private readonly string _fileName;
    private readonly string? _defaultWorkingDirectory;
    private readonly Action<string>? _defaultOutputDataReceived;
    private readonly Action<string>? _defaultErrorDataReceived;

    internal ExternalApplication(
        IProcessHelper processHelper, 
        string fileName,
        string? defaultWorkingDirectory = null,
        Action<string>? defaultOutputDataReceived = null,
        Action<string>? defaultErrorDataReceived = null)
    {
        _processHelper = processHelper;
        _fileName = fileName;
        _defaultWorkingDirectory = defaultWorkingDirectory;
        _defaultOutputDataReceived = defaultOutputDataReceived;
        _defaultErrorDataReceived = defaultErrorDataReceived;
    }

    public async Task<int> StartProcessAsync(
        string? arguments = null,
        string? workingDirectory = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken? cancellationToken = null)
    {
        return await _processHelper.StartProcessAsync(
            _fileName,
            arguments,
            workingDirectory ?? _defaultWorkingDirectory,
            outputDataReceived ?? _defaultOutputDataReceived,
            errorDataReceived ?? _defaultErrorDataReceived,
            cancellationToken);
    }
    
    public async Task<int> StartProcessAsync(
        IEnumerable<string> arguments,
        string? workingDirectory = null,
        Action<string>? outputDataReceived = null,
        Action<string>? errorDataReceived = null,
        CancellationToken? cancellationToken = null)
    {
        return await _processHelper.StartProcessAsync(
            _fileName,
            arguments,
            workingDirectory ?? _defaultWorkingDirectory,
            outputDataReceived ?? _defaultOutputDataReceived,
            errorDataReceived ?? _defaultErrorDataReceived,
            cancellationToken);
    }
    
    public async Task<(int exitCode, string output, string error)> StartProcessWithResultAsync(
        string? arguments = null,
        string? workingDirectory = null,
        CancellationToken? ct = null)
    {
        return await _processHelper.StartProcessWithResultAsync(
            _fileName,
            arguments,
            workingDirectory ?? _defaultWorkingDirectory,
            ct);
    }

    public async Task<(int exitCode, string output, string error)> StartProcessWithResultAsync(
        IEnumerable<string> arguments,
        string? workingDirectory = null,
        CancellationToken? ct = null)
    {
        return await _processHelper.StartProcessWithResultAsync(
            _fileName,
            arguments,
            workingDirectory ?? _defaultWorkingDirectory,
            ct);
    }
}