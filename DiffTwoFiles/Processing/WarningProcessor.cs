namespace DiffTwoFiles.Processing;

public class WarningProcessor : IProcess
{
    public string Process(string input)
    {
        var start = input.IndexOf('(');
        var end = input.IndexOf(')');
        if (start < 0 || end < 0)
            return input;
        
        return input.Remove(start, end - start + 1);
    }
}