namespace DiffTwoFiles.Processing;

public class WarningProcessor : IProcess
{
    public string Process(string input)
    {
        // var nuIdx = input.IndexOf("NU1701", StringComparison.InvariantCultureIgnoreCase);
        // if (nuIdx != -1)
        // {
        //     var endNuIdx = input.IndexOf('[');
        //     return input[nuIdx..(endNuIdx != -1 ? endNuIdx : input.Length)].Trim();
        // }
            
        // skip source code line numbers
        var start = input.IndexOf('(');
        var end = input.IndexOf(')');
        if (start < 0 || end < 0)
            return input;
        
        var res = input.Remove(start, end - start + 1);
        start = res.LastIndexOf('[');
        end = res.LastIndexOf(']');
        if (start < 0 || end < 0 || end <= start + 1)
            return res;
        var output = res.Remove(start, end - start + 1);
        return output;
    }
}