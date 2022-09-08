using System.Diagnostics;
using System.Text;

namespace LibProcess;

public class ProcessArgumentHandler
{
    private readonly IEnumerable<string>? _argumentList;
    private readonly string? _arguments;

    public ProcessArgumentHandler()
    {
        _arguments = null;
        _argumentList = null;
    }
    public ProcessArgumentHandler(string arguments)
    {
        _arguments = arguments;
        _argumentList = null;
    }
    
    public ProcessArgumentHandler(IEnumerable<string> argumentList)
    {
        _argumentList = argumentList;
        _arguments = null;
    }
    
    public string FillStartupInfo(ProcessStartInfo startInfo)
    {
        if (_arguments != null)
        {
            startInfo.Arguments = _arguments;
            return _arguments;
        }

        if (_argumentList != null)
        {
            var sb = new StringBuilder();
            foreach (var arg in _argumentList)
            {
                startInfo.ArgumentList.Add(arg);
                sb.Append('<');
                sb.Append(arg);
                sb.Append('>');
            }
            return sb.ToString();    
        }
        return string.Empty;
    }
    
}