namespace DiffTwoFiles.Services;

public static class WarningStatsCalcService
{
    public static Dictionary<string, int> GetStats(IEnumerable<string> lines)
    {
        var stats = new Dictionary<string, int>();
        foreach (var line in lines)
        {
            var warnStartIdx = line.IndexOf("warning", StringComparison.InvariantCulture);
            if (warnStartIdx < 0)
                continue;
            var warnEndIdx = line.IndexOf(':', warnStartIdx);
            var parts = line[warnStartIdx..warnEndIdx].Split(' ');
            if (parts.Length < 2)
                throw new Exception("Warning stats calculation failed");
            if (!stats.TryAdd(parts[1], 1))
            {
                ++stats[parts[1]];
            }
        }
        return stats;
    }
}