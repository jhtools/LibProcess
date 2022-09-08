using System.Diagnostics;

namespace LibProcess;

public static class ProcessExtensions
{
    public static async Task Kill(this Process process, CancellationToken ct, bool force = true)
    {
        if (force)
        {
            process.Kill();
        }
        else
        {
            process.CloseMainWindow();
            await process.WaitForExitAsync(ct);
        }
    }
}